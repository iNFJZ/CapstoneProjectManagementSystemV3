using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class GroupIdea
    {
        public GroupIdea()
        {
            RegisteredGroups = new HashSet<RegisteredGroup>();
        }

        public int GroupIdeaId { get; set; }
        public int ProfessionId { get; set; }
        public int SpecialtyId { get; set; }
        public string? ProjectEnglishName { get; set; }
        public string? ProjectVietNameseName { get; set; }
        public string Abbreviation { get; set; } = null!;
        public string? Description { get; set; }
        public string ProjectTags { get; set; } = null!;
        public int SemesterId { get; set; }
        public int NumberOfMember { get; set; }
        public int MaxMember { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Profession Profession { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual Specialty Specialty { get; set; } = null!;
        public virtual ICollection<RegisteredGroup> RegisteredGroups { get; set; }
    }
}
