using Infrastructure.Entities.Dto.ViewModel.StaffViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class ProfessionDto : CommonProperty
    {
        public int ProfessionID { get; set; }
        public string ProfessionFullName { get; set; }
        public string ProfessionAbbreviation { get; set; }
        public SemesterDto Semester { get; set; }
        public IList<SpecialtyDto> Specialties { get; set; }
        public IList<SpecialtyWithRowNum> SpecialtyWithRowNum { get; set; }
        public IList<GroupIdeaDto> GroupIdeas { get; set; }
        public IList<FinalGroupDto> FinalGroups { get; set; }
        public IList<StudentDto> Students { get; set; }
        public IList<SupervisorDto> Supervisors { get; set; }
    }
}
