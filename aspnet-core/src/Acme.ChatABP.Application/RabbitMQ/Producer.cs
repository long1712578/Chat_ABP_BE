using Acme.ChatABP.Dtos.Request;
using Acme.ChatABP.Dtos.Response;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Volo.Abp.Application.Services;

namespace Acme.ChatABP.Messages
{
    public class Producer:IProducer
    {

        public void PushMessageToRabbitMQ(MessageRequest content)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "192.168.1.173",
                UserName = "guest",
                Password = "tmt123123@",
                Port = 5672
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "demo-queue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var message = new { nameof = content.SenderName, message = content.Content,time= content.TimeStamp };
                    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                    channel.BasicPublish("", "demo-queue", null, body);
                }
            }
        }
    }
}
