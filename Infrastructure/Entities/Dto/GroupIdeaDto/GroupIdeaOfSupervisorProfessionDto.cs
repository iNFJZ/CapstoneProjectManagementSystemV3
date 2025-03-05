using Infrastructure.Entities.Dto.SpecialtyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class GroupIdeaOfSupervisorProfessionDto
    {
        public ProfessionDto Profession { get; set; }

        public SupervisorDto Supervisor { get; set; }

        public SpecialtyDto Specialty { get; set; }
    }
}
