using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Specialty
    {
        public Specialty()
        {
            FinalGroups = new HashSet<FinalGroup>();
            GroupIdeaOfSupervisorProfessions = new HashSet<GroupIdeaOfSupervisorProfession>();
            GroupIdeas = new HashSet<GroupIdea>();
            Students = new HashSet<Student>();
            WithsNavigation = new HashSet<With>();
            Withs = new HashSet<With>();
        }

        public int SpecialtyId { get; set; }
        public string? SpecialtyAbbreviation { get; set; }
        public string SpecialtyFullName { get; set; } = null!;
        public int ProfessionId { get; set; }
        public int? MaxMember { get; set; }
        public string? CodeOfGroupName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public int? SemesterId { get; set; }

        public virtual Profession Profession { get; set; } = null!;
        public virtual Semester? Semester { get; set; }
        public virtual ICollection<FinalGroup> FinalGroups { get; set; }
        public virtual ICollection<GroupIdeaOfSupervisorProfession> GroupIdeaOfSupervisorProfessions { get; set; }
        public virtual ICollection<GroupIdea> GroupIdeas { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<With> WithsNavigation { get; set; }

        public virtual ICollection<With> Withs { get; set; }
    }
}
