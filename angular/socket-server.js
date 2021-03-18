let express = require('express')
let app = express();
let uniqid = require('uniqid');

let path = require('path');
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


var clients = [];
var busyUsers = [];
var numUsers = 0;
var socketMap = {};
io.on('connection', (socket) => {
    var addedUser = false;
    
    socket.userId="1";
    var userId=socket.handshake.userId;
    if(!socketMap[userId]){
        socketMap[userId]=[];
    }
    
    consumer.on('message', function(message){
        var listMessages = JSON.parse(message.value);
        console.log("message",listMessages.listUserId);
        listMessages.listUserId.forEach(userId=>{
            socket.broadcast.to(userId).emit('add message',{message});
        })
    })
    //Message

    socket.on('add user', (username) => {
        console.log('userId: ', socket.id);
        if (addedUser) return;
        //add customer in list client.
        clients.push({
            id: socket.id,
            username: username,
            busy: false
        });
        // we store the username in the socket session for this client
        socket.username = username;
        ++numUsers;
        addedUser = true;
        socket.emit('login-user-count', {
            numUsers: numUsers
        });
        socket.emit('logged-user', {
            username: socket.username,
            numUsers: numUsers
        });

        setInterval(() => {
            socket.broadcast.emit('client-list', clients);
        }, 3000);
    });

    
});
