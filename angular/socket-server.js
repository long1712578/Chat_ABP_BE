let express = require('express')
let app = express();
let uniqid = require('uniqid');
let path = require('path');
const { InMemorySessionStore } = require("./sessionStore");
const sessionStore = new InMemorySessionStore();

app.use(express.static(__dirname + '/dist/chatApp'));
app.get('/*', (req, res) => res.sendFile(path.join(__dirname)));

var server = require('http').createServer(app);
var io = require('socket.io')(server, {
    cors: {
        origin: "http://localhost:9000",
        methods: ["GET", "POST"],
        allowedHeaders: ["my-custom-header"],
        credentials: true
    }
});

const port = process.env.PORT || 9000;
server.listen(port, () => {
    console.log(`started on port: ${port}`);
});

var kafka=require('kafka-node');
let Consumer=kafka.Consumer,
    client=new kafka.KafkaClient({kafkaHost: '192.168.1.173:9092'}),
    consumer=new Consumer(client,
        [
            {
                topic: 'Kafka-mesage',
                partition:0,
                offset:0
             }
        ],{
            groupId: 'Message',
            autoCommit:false
        });
//middleware
    //sessionID, private, which will be used to authenticate the user upon reconnection
    //user ID, public, which will be used as an identifier to exchange messages
io.use((socket, next) => {
    const sessionID = socket.handshake.sessionID;
    if (sessionID) {
        const session = sessionStore.findSession(sessionID);
        if (session) {
        socket.sessionID = sessionID;
        socket.userId = session.userId;
        return next();
        }
    }
    socket.sessionID = uniqid();
    var senderId;
    //Get senderId from kafka
    // consumer.on('message', function(message){
    //     var value = JSON.parse(message.value);
    //     senderId=value.senderId;
    // })
    socket.userId = "123";//senderId of kafka
    next();
});
                
var clientOnlines = [];
var numUsers = 0;
io.on('connection', (socket) => {
    console.log(`a user has connection ${socket.senderId}`)
    sessionStore.saveSession(socket.sessionID, {
        userId: socket.userId
      });
    //save user online
    clientOnlines.push(socket.userId);
    //session
    socket.emit("session", {
    sessionID: socket.sessionID,
    userId: socket.userId,
    });
    // join the "userID" room
    socket.join(socket.userId);
    //send message to user in conversation
    consumer.on('message', function(message){
        var listMessages = JSON.parse(message.value);
        console.log("message",listMessages.listUserId);
        var content=listMessages.content;
        listMessages.listUserId.forEach(userId=>{
            socket.broadcast.to(userId).emit('add message',{content});
        })
    })
    //disconnection socket
    socket.on('disconnection', async() => {
        const matchingSockets=await io.in(socket.userId).allSockets();
        console.log('matching socket', matchingSockets);
        const isDisconnection=matchingSockets.size===0;
        if(isDisconnection){
            console.log('disconnection');
            socket.broadcast.emit("user disconnected", socket.userId);
            clientOnlines= clientOnlines.filter(e => e !== socket.userId);
        }
    })
  
});
