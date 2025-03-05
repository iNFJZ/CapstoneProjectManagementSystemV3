using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class GroupIdeasOfSupervisor
    {
        public GroupIdeasOfSupervisor()
        {
            GroupIdeaOfSupervisorProfessions = new HashSet<GroupIdeaOfSupervisorProfession>();
            RegisterdGroupSupervisors = new HashSet<RegisterdGroupSupervisor>();
        }

        public int GroupIdeaId { get; set; }
        public string? ProjectEnglishName { get; set; }
        public string? ProjectVietNameseName { get; set; }
        public string? Abbreviation { get; set; }
        public string? Description { get; set; }
        public string? ProjectTags { get; set; }
        public int SemesterId { get; set; }
        public int NumberOfMember { get; set; }
        public int MaxMember { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string SupervisorId { get; set; } = null!;

        public virtual Semester Semester { get; set; } = null!;
        public virtual Supervisor Supervisor { get; set; } = null!;
        public virtual ICollection<GroupIdeaOfSupervisorProfession> GroupIdeaOfSupervisorProfessions { get; set; }
        public virtual ICollection<RegisterdGroupSupervisor> RegisterdGroupSupervisors { get; set; }
    }
}
