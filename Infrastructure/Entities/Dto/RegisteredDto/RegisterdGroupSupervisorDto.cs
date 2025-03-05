using Infrastructure.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.RegisteredDto
{
    public class RegisterdGroupSupervisorDto : CommonProperty
    {
        public Supervisor Supervisor { get; set; }

        public SupervisorForAssigning SupervisorForAssigning { get; set; }

        public RegisteredGroup RegisteredGroup { get; set; }

        public GroupIdeasOfSupervisor? GroupIdeaOfSupervisor { get; set; }

        public int Status { get; set; }

        public bool? IsAssigned { get; set; }
    }
}
