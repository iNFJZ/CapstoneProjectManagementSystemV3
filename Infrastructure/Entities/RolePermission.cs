using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class RolePermission
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Permission Permission { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}
