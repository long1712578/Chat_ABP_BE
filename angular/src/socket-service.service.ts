import { Injectable } from '@angular/core';
import { Observable,from } from 'rxjs';
import * as io from 'socket.io-client';
import { Socket } from 'ngx-socket-io';

@Injectable({
  providedIn: 'root'
})
export class SocketServiceService {
    private connected = false;
    public connectedusers: any;

    constructor(private socket: Socket) {
    }

    // tslint:disable-next-line:typedef
    SetUserName(username: string | null) {
        this.socket.emit('add user', username);
        return Observable.create((observer: any) => {
            this.socket.on('logged-user', (data: any) => {
                this.connected = true;
                observer.next(data);
            });
        });
    }
    // tslint:disable-next-line:typedef
    public RemoveUser() {
        this.socket.emit('disconnect');
    }

    // tslint:disable-next-line:typedef
    public BroadCastMessage(message: any) {
        this.socket.emit('new-broadcast-message', message);
    }

    // tslint:disable-next-line:typedef
    // tslint:disable-next-line:no-shadowed-variable
    // tslint:disable-next-line:typedef
    // tslint:disable-next-line:no-shadowed-variable
    // tslint:disable-next-line:typedef
    public SendMessage(message: any, from: any, to: any) {
        this.socket.emit('send-message', {
            toid: to,
            message,
            fromname: from
        });
    }

    // tslint:disable-next-line:typedef
    public GetMessages() {
        return Observable.create((observer: { next: (arg0: any) => void; }) => {
            this.socket.on('receive-message', (message: any) => {
                observer.next(message);
            });
        });
    }
    // tslint:disable-next-line:typedef
    public GetConnectedUsers() {
        return Observable.create((observer: any) => {
            this.socket.on('client-list', (data: any[]) => {
                observer.next(data);
            });
        });
    }
    // tslint:disable-next-line:typedef
    public GetSession() {
        return Observable.create((observer: any) => {
            this.socket.on('session', (data: any) => {
                localStorage.setItem('sessionId', data.sessionId);
                observer.next(data);
            });
        });
    }
    /***
     * Section Video call
     * following requests are used for video call
     */

    // tslint:disable-next-line:typedef
    public VideoCallRequest(from: any, to: any) {
        this.socket.emit('video-call', {
            fromname: from,
            toid: to
        });
    }
    // tslint:disable-next-line:typedef
    public OnVideoCallRequest() {
        return Observable.create((observer: { next: (arg0: any) => void; }) => {
            this.socket.on('video-call', (data: any) => {
                observer.next(data);
            });
        });
    }
    // tslint:disable-next-line:typedef
    // tslint:disable-next-line:no-shadowed-variable
    public VideoCallAccepted(from: any, to: any) {
        this.socket.emit('video-call-accept', {
            fromname: from,
            toid: to
        });
    }
    // tslint:disable-next-line:typedef
    public OnVideoCallAccepted() {
        return Observable.create((observer: { next: (arg0: any) => void; }) => {
            this.socket.on('video-call-accept', (data: any) => {
                observer.next(data);
            });
        });
    }
    // tslint:disable-next-line:typedef
    public BusyNow() {
        this.socket.emit('busy-user');
    }
    // tslint:disable-next-line:typedef
    public GetBusyUsers() {
        this.socket.emit('get-busy-user');
        return Observable.create((observer: { next: (arg0: any) => void; }) => {
            this.socket.on('get-busy-user', (data: any) => {
                observer.next(data);
            });
        });
    }
    // tslint:disable-next-line:typedef
    public EndVideoCall(from: any, to: any, toname: any) {
        this.socket.emit('end-video-call', {
            fromname: from,
            toid: to,
            toname: toname
        });
    }
    // tslint:disable-next-line:typedef
    public OnVideoCallEnded() {
        this.socket.emit('get-busy-user');
        return Observable.create((observer: { next: (arg0: any) => void; }) => {
            this.socket.on('video-call-ended', (data: any) => {
                observer.next(data);
            });
        });
    }
    // tslint:disable-next-line:typedef
    public VideoCallRejected(from: any, to: any) {
        this.socket.emit('video-call-reject', {
            fromname: from,
            toid: to
        });
    }
    public OnVideoCallRejected() {
        return Observable.create((observer: { next: (arg0: any) => void; }) => {
            this.socket.on('video-call-reject', (data: any) => {
                observer.next(data);
            });
        });
    }
    /**
     * 
     * @param candidate or @param description for video call
     * need to send remote user id
     */
    public SendCallRequest(val: any, type: string, uid: any) {
        var data;
        if (type == 'desc') {
            data = {
                toid: uid,
                desc: val
            }
        } else {
            data = {
                toid: uid,
                candidate: val
            }
        }
        this.socket.emit('call-request', data);
    }
    public ReceiveCallRequest() {
        return Observable.create((observer: { next: (arg0: any) => void; }) => {
            this.socket.on('call-request', (data: any) => {
                observer.next(data);
            });
        });
    }
}
