using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class RolePermissionDto : CommonProperty
    {
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public RoleDto Role { get; set; }
        public PermissionDto Permission { get; set; }
    }
}
