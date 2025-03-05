using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class With
    {
        public With()
        {
            Specialties = new HashSet<Specialty>();
        }

        public int WithId { get; set; }
        public int? ProfessionId { get; set; }
        public int? SpecialtyId { get; set; }

        public virtual Profession? Profession { get; set; }
        public virtual Specialty? Specialty { get; set; }

        public virtual ICollection<Specialty> Specialties { get; set; }
    }
}
