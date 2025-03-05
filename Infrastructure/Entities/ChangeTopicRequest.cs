using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class ChangeTopicRequest
    {
        public int RequestId { get; set; }
        public string? OldTopicNameEnglish { get; set; }
        public string? OldTopicNameVietNamese { get; set; }
        public string? OldAbbreviation { get; set; }
        public string? NewTopicNameEnglish { get; set; }
        public string? NewTopicNameVietNamese { get; set; }
        public string? NewAbbreviation { get; set; }
        public string EmailSuperVisor { get; set; } = null!;
        public string ReasonChangeTopic { get; set; } = null!;
        public int FinalGroupId { get; set; }
        public string? StaffComment { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual FinalGroup FinalGroup { get; set; } = null!;
    }
}
