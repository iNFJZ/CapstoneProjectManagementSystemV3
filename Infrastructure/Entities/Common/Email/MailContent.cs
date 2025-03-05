using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Common.Email
{
    public class MailContent
    {
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
