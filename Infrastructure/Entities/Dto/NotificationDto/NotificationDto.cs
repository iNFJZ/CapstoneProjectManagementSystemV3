using Infrastructure.Entities.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class NotificationDto : CommonProperty
    {
        public int NotificationID { get; set; }
        public bool Readed { get; set; }
        public string NotificationContent { get; set; }
        public string AttachedLink { get; set; }
        public UserDto User { get; set; }
    }
}
