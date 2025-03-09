using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class RegisteredGroupDto : CommonProperty
    {
        public GroupIdea GroupIdea { get; set; }

        public RegisteredGroup RegisteredGroup { get; set; }

        public Supervisor Supervisor { get; set; }

        public User Leader { get; set; }
        public List<User> Members { get; set; }

        public int NumberOfMember { get; set; }

        public List<GroupIdeaOfSupervisorDto> GroupIdeaOfSupervisors { get; set; }
    }
}
