using System.Collections.Generic;
using Infrastructure.Entities;

namespace Infrastructure.Custom
{
    public class SupervisorProfessionDto
    {

        public Supervisor Supervisor { get; set; }

        public List<Profession> Professions { get; set; }

        public bool IsDevHead { get; set; }

        public int MaxGroup { get; set; }



    }
}
