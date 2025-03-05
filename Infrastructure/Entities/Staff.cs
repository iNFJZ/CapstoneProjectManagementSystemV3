using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Staff
    {
        public Staff()
        {
            News = new HashSet<News>();
            UserGuides = new HashSet<UserGuide>();
        }

        public string StaffId { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual User StaffNavigation { get; set; } = null!;
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<UserGuide> UserGuides { get; set; }
    }
}
