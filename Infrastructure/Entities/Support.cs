using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Support
    {
        public int SupportId { get; set; }
        public string StudentId { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? ContactEmail { get; set; }
        public string SupportMessage { get; set; } = null!;
        public string? Attachment { get; set; }
        public string? Title { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? ReplyMessage { get; set; }
        public DateTime? ReplyAt { get; set; }

        public virtual Student Student { get; set; } = null!;
    }
}
