using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class StudentGroupIdea
    {
        public string StudentId { get; set; } = null!;
        public int GroupIdeaId { get; set; }
        public int? Status { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual GroupIdea GroupIdea { get; set; } = null!;
        public virtual Student Student { get; set; } = null!;
    }
}
