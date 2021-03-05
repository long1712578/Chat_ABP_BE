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
    public  class ChatABPController : AbpController
    {
       
        public ChatABPController(IChatAppServices chatAppService, IProducer producer)
        {
            //LocalizationResource = typeof(ChatABPResource);
           
        }
        
    }
}