using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class ReportMaterial
    {
        public int ReportId { get; set; }
        public string ReportTile { get; set; } = null!;
        public string? ReportContent { get; set; }
        public int? Status { get; set; }
        public DateTime DueDate { get; set; }
        public int FinalGroupId { get; set; }
        public string? SubmissionComment { get; set; }
        public string? SubmissionAttachment { get; set; }
        public string? SupervisorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual FinalGroup FinalGroup { get; set; } = null!;
        public virtual Supervisor? Supervisor { get; set; }
    }
}
