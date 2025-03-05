using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class SupervisorProfession
    {
        public string SupervisorId { get; set; } = null!;
        public bool? IsDevHead { get; set; }
        public int? MaxGroup { get; set; }
        public int ProfessionId { get; set; }

        public virtual Profession Profession { get; set; } = null!;
        public virtual Supervisor Supervisor { get; set; } = null!;
    }
}
