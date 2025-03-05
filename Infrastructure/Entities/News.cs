using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class News
    {
        public int NewsId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string StaffId { get; set; } = null!;
        public bool? Pin { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? SemesterId { get; set; }
        public string? FileName { get; set; }
        public byte[]? AttachedFile { get; set; }
        public bool? TypeSupport { get; set; }

        public virtual Semester? Semester { get; set; }
        public virtual Staff Staff { get; set; } = null!;
    }
}
