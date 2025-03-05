using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.SupervisorDto
{
    public class SupervisorProfessionDto
    {
        public Supervisor Supervisor { get; set; }

        public Profession Profession { get; set; }

        public bool IsDevHead { get; set; }

        public int MaxGroup { get; set; }
    }
}
