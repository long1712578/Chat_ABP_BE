using Acme.ChatABP.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.ChatABP.Messages
{
    public interface IChatAppServices: IApplicationService
    {
        void CreateAsync(MessageRequest input);
    }
}
