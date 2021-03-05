using Acme.ChatABP.Dtos.Request;
using Acme.ChatABP.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.ChatABP.Messages
{
    public interface IProducer
    {
        public void PushMessageToRabbitMQ(MessageRequest content);
    }
}
