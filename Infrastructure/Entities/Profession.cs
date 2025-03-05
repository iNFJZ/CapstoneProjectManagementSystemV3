using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Profession
    {
        public Profession()
        {
            FinalGroups = new HashSet<FinalGroup>();
            GroupIdeaOfSupervisorProfessions = new HashSet<GroupIdeaOfSupervisorProfession>();
            GroupIdeas = new HashSet<GroupIdea>();
            Specialties = new HashSet<Specialty>();
            Students = new HashSet<Student>();
            SupervisorProfessions = new HashSet<SupervisorProfession>();
            Withs = new HashSet<With>();
        }

        public int ProfessionId { get; set; }
        public string? ProfessionAbbreviation { get; set; }
        public string ProfessionFullName { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? SemesterId { get; set; }

        public virtual Semester? Semester { get; set; }
        public virtual ICollection<FinalGroup> FinalGroups { get; set; }
        public virtual ICollection<GroupIdeaOfSupervisorProfession> GroupIdeaOfSupervisorProfessions { get; set; }
        public virtual ICollection<GroupIdea> GroupIdeas { get; set; }
        public virtual ICollection<Specialty> Specialties { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<SupervisorProfession> SupervisorProfessions { get; set; }
        public virtual ICollection<With> Withs { get; set; }
    }
}
