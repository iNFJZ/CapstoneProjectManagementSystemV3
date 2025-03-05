using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class UserGuide
    {
        public int UserGuideId { get; set; }
        public string UserGuideLink { get; set; } = null!;
        public string StaffId { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Staff Staff { get; set; } = null!;
    }
}
