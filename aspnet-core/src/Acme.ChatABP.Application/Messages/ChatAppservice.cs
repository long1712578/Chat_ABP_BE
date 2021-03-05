using Acme.ChatABP.Dtos.Request;
using Acme.ChatABP.Dtos.Response;
using Acme.ChatABP.Entities;
using Acme.ChatABP.RabbitMQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace Acme.ChatABP.Messages
{
    public class ChatAppservice : ApplicationService, IChatAppServices, ITransientDependency
    {
        private readonly IRepository<Message, Guid> _messageRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly IProducer _producer;
        
        

       public ChatAppservice(IRepository<Message, Guid> messageRepository , IIdentityUserRepository identityUserRepository, ILookupNormalizer lookupNormalizer, IDistributedEventBus distributedEventBus, IProducer producer)
        {
            _messageRepository = messageRepository;
            _identityUserRepository = identityUserRepository;
            _lookupNormalizer = lookupNormalizer;
            _distributedEventBus = distributedEventBus;
            _producer = producer;
           
           
        }

        public void CreateAsync(MessageRequest input)
        {
            var message = new Message(GuidGenerator.Create(), input.SenderName
                    , input.Content, input.TimeStamp);
            _producer.PushMessageToRabbitMQ(input);
        }
    }
}
