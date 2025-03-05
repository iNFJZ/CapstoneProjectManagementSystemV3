using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Permission
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
