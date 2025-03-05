using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class FinalGroup
    {
        public FinalGroup()
        {
            ChangeTopicRequests = new HashSet<ChangeTopicRequest>();
            DefenceSchedules = new HashSet<DefenceSchedule>();
            ReportMaterials = new HashSet<ReportMaterial>();
            Students = new HashSet<Student>();
        }

        public int FinalGroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public int? MaxMember { get; set; }
        public int? NumberOfMember { get; set; }
        public int ProfessionId { get; set; }
        public int SpecialtyId { get; set; }
        public string? ProjectEnglishName { get; set; }
        public string? ProjectVietNameseName { get; set; }
        public string Abbreviation { get; set; } = null!;
        public int SemesterId { get; set; }
        public string? SupervisorId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsConfirmFinalReport { get; set; }

        public virtual Profession Profession { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual Specialty Specialty { get; set; } = null!;
        public virtual Supervisor? Supervisor { get; set; }
        public virtual ICollection<ChangeTopicRequest> ChangeTopicRequests { get; set; }
        public virtual ICollection<DefenceSchedule> DefenceSchedules { get; set; }
        public virtual ICollection<ReportMaterial> ReportMaterials { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
