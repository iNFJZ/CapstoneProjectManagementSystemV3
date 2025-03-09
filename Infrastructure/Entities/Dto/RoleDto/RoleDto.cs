using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class RoleDto : CommonProperty
    {
        public int Role_ID { get; set; }
        public string RoleName { get; set; }
        public IList<UserDto> Users { get; set; }
        public IList<RolePermissionDto> RolePermissions { get; set; }
    }
}
