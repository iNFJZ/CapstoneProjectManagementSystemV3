using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class User
    {
        public User()
        {
            Notifications = new HashSet<Notification>();
        }

        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FptEmail { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string? Avatar { get; set; }
        public int? Gender { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Role? Role { get; set; }
        public virtual AffiliateAccount? AffiliateAccount { get; set; }
        public virtual Staff? Staff { get; set; }
        public virtual Student? Student { get; set; }
        public virtual Supervisor? Supervisor { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
