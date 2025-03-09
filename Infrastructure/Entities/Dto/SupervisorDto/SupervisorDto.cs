using Infrastructure.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class SupervisorDto : CommonProperty
    {
        public bool IsActive { get; set; }
        public string FieldSetting { get; set; }
        public string FeEduEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string SupervisorID { get; set; }
        public string SelfDescription { get; set; }
        public ProfessionDto Profession { get; set; }
        public List<ProfessionDto> Professions { get; set; }

        public List<SupervisorProfessionDto> SupervisorProfessions { get; set; }
        public UserDto User { get; set; }
        public UserDto SupervisorNavigation { get; set; }
        public IList<ChangeTopicRequestDto> ChangeTopicRequests { get; set; }
        public IList<FinalGroupDto> FinalGroups { get; set; }
        public IList<RegisterdGroupSupervisorDto> RegisteredGroupSupervisors { get; set; }
        public IList<ReportMaterialDto> ReportMaterials { get; set; }
    }
}
