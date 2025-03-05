using Infrastructure.Entities.Dto.SpecialtyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class ConfigurationDto
    {
        public SpecialtyDto Specialty { get; set; }
        public IList<WithDto> Withs { get; set; }
    }
}
