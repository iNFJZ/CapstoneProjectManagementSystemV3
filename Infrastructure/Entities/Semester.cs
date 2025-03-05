using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Semester
    {
        public Semester()
        {
            FinalGroups = new HashSet<FinalGroup>();
            GroupIdeas = new HashSet<GroupIdea>();
            GroupIdeasOfSupervisors = new HashSet<GroupIdeasOfSupervisor>();
            News = new HashSet<News>();
            Professions = new HashSet<Profession>();
            Specialties = new HashSet<Specialty>();
            Students = new HashSet<Student>();
        }

        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = null!;
        public string SemesterCode { get; set; } = null!;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? StatusCloseBit { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? ShowGroupName { get; set; }
        public DateTime? DeadlineChangeIdea { get; set; }
        public DateTime? DeadlineRegisterGroup { get; set; }
        public string? SubjectMailTemplate { get; set; }
        public string? BodyMailTemplate { get; set; }
        public bool? IsConfirmationOfDevHeadNeeded { get; set; }

        public virtual ICollection<FinalGroup> FinalGroups { get; set; }
        public virtual ICollection<GroupIdea> GroupIdeas { get; set; }
        public virtual ICollection<GroupIdeasOfSupervisor> GroupIdeasOfSupervisors { get; set; }
        public virtual ICollection<News> News { get; set; }
        public virtual ICollection<Profession> Professions { get; set; }
        public virtual ICollection<Specialty> Specialties { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
