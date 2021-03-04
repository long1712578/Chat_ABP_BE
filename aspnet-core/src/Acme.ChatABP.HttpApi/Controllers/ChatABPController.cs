using Acme.ChatABP.Dtos.Request;
using Acme.ChatABP.Localization;
using Acme.ChatABP.Messages;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace Acme.ChatABP.Controllers
{
    /* Inherit your controllers from this class.
     */
    public  class ChatABPController : AbpController, IChatAppServices
    {
        private readonly IChatAppServices _chatAppService;
        private readonly Producer _producer;
        public ChatABPController(IChatAppServices chatAppService, Producer producer)
        {
            //LocalizationResource = typeof(ChatABPResource);
            _chatAppService = chatAppService;
            _producer = producer;
        }
        public async Task CreateAsync(MessageRequest input)
        {
          
            await _chatAppService.CreateAsync(input);
        }
    }
}