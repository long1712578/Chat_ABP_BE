export class Message {
    Content: string | undefined;
    Type!: string;
    Status: string | undefined;
    SenderName: string | undefined;
    updateAt: string | undefined;
    listUserId: string[] | undefined;
    senderId: string | undefined;
}
