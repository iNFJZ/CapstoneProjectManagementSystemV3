using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class UserCreateRequest
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string FptEmail { get; set; }
        public int? Gender { get; set; }
        public int RoleID { get; set; }
    }
}
