
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Acme.ChatABP.Dtos.Request;
using Microsoft.Extensions.Hosting;

namespace Acme.ChatABP.Messages
{
    public interface IKafkaProducer
    {
        public  Task MessageToKafka(MessageRequest message);
    }
}
