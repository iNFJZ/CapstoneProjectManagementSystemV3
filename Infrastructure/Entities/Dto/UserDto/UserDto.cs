using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class UserDto : CommonProperty
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FptEmail { get; set; }
        public string? Avatar { get; set; }
        public int? Gender { get; set; }
        public AffiliateAccountDto AffiliateAccount { get; set; }
        public int RoleID { get; set; }
        public StaffDto Staff { get; set; }
        public StudentDto Student { get; set; }
        public SupervisorDto Supervisor { get; set; }
        public RoleDto Role { get; set; }
        public IList<NotificationDto> Notifications { get; set; }
    }
}
