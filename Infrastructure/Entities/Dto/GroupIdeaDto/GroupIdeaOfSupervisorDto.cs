using Infrastructure.Entities.Dto.RegisteredDto;
using Infrastructure.Entities.Dto.SemesterDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class GroupIdeaOfSupervisorDto : CommonProperty
    {
        public int GroupIdeaID { get; set; }
        public string ProjectEnglishName { get; set; }
        public string ProjectVietNameseName { get; set; }
        public string Abrrevation { get; set; }
        public string Description { get; set; }
        public string ProjectTags { get; set; }
        public int NumberOfMember { get; set; }
        public int MaxMember { get; set; }
        public SemesterDto Semester { get; set; }
        public bool? IsActive { get; set; }

        public SupervisorDto Supervisor { get; set; }

        public IList<GroupIdeaOfSupervisorProfessionDto> GroupIdeaOfSupervisorProfessions { get; set; }

        public IList<RegisteredGroupDto> RegisteredGroups { get; set; }
    }
}
