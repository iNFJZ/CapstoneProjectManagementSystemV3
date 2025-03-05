using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class ChangeFinalGroupRequest
    {
        public int ChangeFinalGroupRequestId { get; set; }
        public string FromStudentId { get; set; } = null!;
        public string ToStudentId { get; set; } = null!;
        public int? StatusOfTo { get; set; }
        public int? StatusOfStaff { get; set; }
        public string? StaffComment { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Student FromStudent { get; set; } = null!;
        public virtual Student ToStudent { get; set; } = null!;
    }
}
