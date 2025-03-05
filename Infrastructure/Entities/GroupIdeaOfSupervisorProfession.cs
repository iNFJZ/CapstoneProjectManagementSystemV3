using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class GroupIdeaOfSupervisorProfession
    {
        public int GroupIdeaId { get; set; }
        public int? SpecialtyId { get; set; }
        public int ProfessionId { get; set; }

        public virtual GroupIdeasOfSupervisor GroupIdea { get; set; } = null!;
        public virtual Profession Profession { get; set; } = null!;
        public virtual Specialty? Specialty { get; set; }
    }
}
