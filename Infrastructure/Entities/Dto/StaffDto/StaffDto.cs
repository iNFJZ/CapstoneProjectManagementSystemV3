using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class StaffDto : CommonProperty
    {
        public string StaffID { get; set; }
        public UserDto User { get; set; }
        public IList<UserGuideDto> UserGuides { get; set; }
        public IList<NewsDto> News { get; set; }
        public IList<SupportDto> Supports { get; set; }
        public IList<ChangeTopicRequestDto> ChangeTopicRequests { get; set; }
    }
}
