using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public class Configuration
    {
        public Specialty Specialty { get; set; }
        public IList<With> Withs { get; set; }
    }
}
