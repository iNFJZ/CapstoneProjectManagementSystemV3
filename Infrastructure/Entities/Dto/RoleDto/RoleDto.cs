using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.RoleDto
{
    public class RoleDto : CommonProperty
    {
        public int Role_ID { get; set; }
        public string RoleName { get; set; }
        public IList<User> Users { get; set; }
        public IList<RolePermission> RolePermissions { get; set; }
    }
}
