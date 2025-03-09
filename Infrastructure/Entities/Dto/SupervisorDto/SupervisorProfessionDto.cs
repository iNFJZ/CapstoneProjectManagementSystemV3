using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class SupervisorProfessionDto : CommonProperty
    {
        public SupervisorDto Supervisor { get; set; }

        public ProfessionDto Profession { get; set; }

        public bool IsDevHead { get; set; }

        public int MaxGroup { get; set; }
    }
}
