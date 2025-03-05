using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class RegisterdGroupSupervisor
    {
        public int RegisteredGroupId { get; set; }
        public string SupervisorId { get; set; } = null!;
        public int? GroupIdeaOfSupervisorId { get; set; }
        public bool? IsAssigned { get; set; }
        public int? Status { get; set; }

        public virtual GroupIdeasOfSupervisor? GroupIdeaOfSupervisor { get; set; }
        public virtual RegisteredGroup RegisteredGroup { get; set; } = null!;
        public virtual Supervisor Supervisor { get; set; } = null!;
    }
}
