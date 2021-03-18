using Acme.ChatABP.Dtos.Request;
using Acme.ChatABP.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Acme.ChatABP.RabbitMQ
{
    public class KafkaProducer: IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private IProducer<Null, string> _producer;

        public KafkaProducer(ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            var config = new ProducerConfig()
            {
                BootstrapServers = "192.168.1.173:9092"
            };
            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task MessageToKafka(MessageRequest message)
        {

            await _producer.ProduceAsync("message", new Message<Null, string>()
            {
                Value = JsonSerializer.Serialize(message)
        })  ; 
            _producer.Flush(TimeSpan.FromSeconds(10)); 
        }
    }
}
