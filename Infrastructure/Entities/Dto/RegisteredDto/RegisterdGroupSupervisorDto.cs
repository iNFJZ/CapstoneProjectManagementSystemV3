using Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class RegisterdGroupSupervisorDto : CommonProperty
    {
        public SupervisorDto Supervisor { get; set; }

        public SupervisorForAssigning SupervisorForAssigning { get; set; }

        public RegisteredGroupDto RegisteredGroup { get; set; }

        public GroupIdeaOfSupervisorDto? GroupIdeaOfSupervisor { get; set; }

        public int Status { get; set; }

        public bool? IsAssigned { get; set; }
    }
}
