using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel
{
    public class SupervisorWithRowNum
    {
        public int RowNum { get; set; }
        public string SupervisorID { get; set; }

        public string PhoneNumber { get; set; }
        public int MaxGroup { get; set; }

        public bool? IsActive { get; set; }
        public bool IsDevHead { get; set; }

        public string FeEduEmail { get; set; }

        public string PersonalEmail { get; set; }
        public User User { get; set; }
        public int ProfessionId { get; set; }
        public string SelfDescription { get; set; }

        public string FieldSetting { get; set; }

        public Student Student { get; set; }
        public Profession Profession { get; set; }
        public List<SupervisorProfession> SupervisorProfessions { get; set; }
        public SupervisorProfession SupervisorProfession { get; set; }
        public IList<FinalGroup> FinalGroups { get; set; }
        public IList<ChangeTopicRequest> ChangeTopicRequests { get; set; }
        public IList<ReportMaterial> ReportMaterials { get; set; }

        public IList<RegisterdGroupSupervisor> RegisterdGroupSupervisors { get; set; }
    }
}
