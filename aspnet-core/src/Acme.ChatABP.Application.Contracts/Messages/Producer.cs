using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Acme.ChatABP.Messages
{
    public  class Producer
    {
         public void Push()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("user-messages",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = new { Pattern = "message_printed", Data = "Lan dep trai" };
            var body = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            })
            );

            channel.BasicPublish("", "user-messages", null, body);
        }
    }
}
