using Infrastructure.Custom;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.ViewModel.AdminViewModel
{
    public class UserWithRowNum
    {
        public int RowNum { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FptEmail { get; set; }
        public string Avatar { get; set; }
        public int? Gender { get; set; }
        public AffiliateAccountDto AffiliateAccount { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public StaffDto Staff { get; set; }
        public StudentDto Student { get; set; }
        public Supervisor Supervisor { get; set; }
        public Role Role { get; set; }
        public DateTime? Created_At { get; set; }
        public IList<NotificationDto> Notifications { get; set; }
    }
}
