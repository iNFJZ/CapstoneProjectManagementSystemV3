using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.StaffDto
{
    public class StaffDto : CommonProperty
    {
        public string StaffID { get; set; }
        public User User { get; set; }
        public IList<UserGuide> UserGuides { get; set; }
        public IList<News> News { get; set; }
        public IList<Support> Supports { get; set; }
        public IList<ChangeTopicRequest> ChangeTopicRequests { get; set; }
    }
}
