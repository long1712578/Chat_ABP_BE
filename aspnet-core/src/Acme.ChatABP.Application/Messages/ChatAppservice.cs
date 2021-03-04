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
using Volo.Abp.Domain.Repositories;
using Volo.Abp.EventBus;
using Volo.Abp.EventBus.Distributed;
using Volo.Abp.Identity;

namespace Acme.ChatABP.Messages
{
    public class ChatAppservice: ApplicationService,IChatAppServices
    {
        private readonly IRepository<Message, Guid> _messageRepository;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly ILookupNormalizer _lookupNormalizer;
        private readonly IEventBus _distributedEventBus;
        

       public ChatAppservice(IRepository<Message, Guid> messageRepository , IIdentityUserRepository identityUserRepository, ILookupNormalizer lookupNormalizer, IEventBus distributedEventBus)
        {
            _messageRepository = messageRepository;
            _identityUserRepository = identityUserRepository;
            _lookupNormalizer = lookupNormalizer;
            _distributedEventBus = distributedEventBus;
           
           
        }


        public async Task CreateAsync(MessageRequest input)
        {
            

            var message = new Message(GuidGenerator.Create(), input.SenderName
                    , input.Content, input.TimeStamp);
            //await _messageRepository.InsertAsync(message);
            await _distributedEventBus.PublishAsync(new ReceivedMessageEto(GuidGenerator.Create(), input.SenderName, input.Content));
        }
    }
}
