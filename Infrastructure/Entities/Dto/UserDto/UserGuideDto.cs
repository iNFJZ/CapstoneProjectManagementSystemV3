using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.UserDto
{
    public class UserGuideDto : CommonProperty
    {
        public int UserGuideID { get; set; }
        public string UserGuideLink { get; set; }
        public Staff Staff { get; set; }
    }
}
