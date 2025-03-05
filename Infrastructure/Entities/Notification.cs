using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public bool? Readed { get; set; }
        public string UserId { get; set; } = null!;
        public string NotificationContent { get; set; } = null!;
        public string? AttachedLink { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
