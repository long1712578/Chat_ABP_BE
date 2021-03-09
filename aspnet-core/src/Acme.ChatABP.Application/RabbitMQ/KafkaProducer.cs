using Acme.ChatABP.Dtos.Request;
using Acme.ChatABP.Messages;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
                Value = "gui lan"
            }); ; 
            _producer.Flush(TimeSpan.FromSeconds(10)); 
        }
       /* public async Task StartAsync(CancellationToken cancellationToken)
        {
            var value = "Hello World long";
            await _producer.ProduceAsync("message", new Message<Null, string>()
            {
                Value = value
            }, cancellationToken);
            _producer.Flush(TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }*/
    }
}
