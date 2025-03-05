using Infrastructure.Custom;
using Infrastructure.Entities.Dto.SpecialtyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class WithDto
    {
        public int With_ID { get; set; }
        public ProfessionDto Profession { get; set; }
        public SpecialtyDto Specialty { get; set; }
    }
}
