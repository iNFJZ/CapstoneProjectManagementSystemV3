using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class PermissionDto : CommonProperty
    {
        public int PermissionID { get; set; }
        public string PermissionName { get; set; }
        public IList<RolePermissionDto> RolePermissions { get; set; }
    }
}
