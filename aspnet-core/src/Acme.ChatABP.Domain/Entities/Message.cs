using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.ChatABP.Entities
{
    public class Message: AuditedAggregateRoot<Guid>
    {
        public string SenderName { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; }

        protected Message()
        {

        }

        public Message(Guid id,  string name, string content, DateTime timeStamp)
        {
            Id = id;
            SenderName = name;
            Content = content;
            TimeStamp = timeStamp;
        }
    }
}
