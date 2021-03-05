using Acme.ChatABP.Dtos.Request;
using Acme.ChatABP.Messages;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EventBus.Distributed;

namespace Acme.ChatABP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : AbpController,ITransientDependency
    {
        private readonly IProducer _producer;
        private readonly IDistributedEventBus _distributedEventBus;
        private readonly IChatAppServices _chatAppServices;

        public ChatController(IProducer producer, IDistributedEventBus distributedEventBus, IChatAppServices chatAppServices)
        {
            _producer = producer;
            _distributedEventBus = distributedEventBus;
            _chatAppServices = chatAppServices;
        }

        [HttpPost("add-mesage")]
        public async Task<IActionResult> SendMessageAsync([FromBody] MessageRequest input)
        {
            _chatAppServices.CreateAsync(input);
            //await _distributedEventBus.PublishAsync("abc");
            return Ok();
        }

    }
}
