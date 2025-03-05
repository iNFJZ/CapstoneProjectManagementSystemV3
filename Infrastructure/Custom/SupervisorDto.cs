using System.Collections.Generic;
using Infrastructure.Entities;

namespace Infrastructure.Custom
{
    public class SupervisorDto
    {
        public bool IsActive { get; set; }
        public bool IsDevHead { get; set; }
        public string FieldSetting { get; set; }
        public string FeEduEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int MaxGroup { get; set; }
        public string SupervisorID { get; set; }
        public string SelfDescription { get; set; }
        public ProfessionDto Profession { get; set; }
        public List<ProfessionDto> Professions { get; set; }
        public List<SupervisorProfession> SupervisorProfessions { get; set; }
        public User User { get; set; }
        public IList<ChangeTopicRequest> ChangeTopicRequests { get; set; }
        public IList<FinalGroup> FinalGroups { get; set; }
        public IList<RegisterdGroupSupervisor> RegisterdGroupSupervisors { get; set; }
        public IList<ReportMaterial> ReportMaterials { get; set; }
    }
}
