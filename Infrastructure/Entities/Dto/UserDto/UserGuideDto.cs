using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class UserGuideDto : CommonProperty
    {
        public int UserGuideID { get; set; }
        public string UserGuideLink { get; set; }
        public StaffDto Staff { get; set; }
    }
}
