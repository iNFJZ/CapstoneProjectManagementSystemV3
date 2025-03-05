using Infrastructure.ViewModel.StaffViewModel;
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
        public Semester Semester { get; set; }
        public IList<Specialty> Specialties { get; set; }
        public IList<SpecialtyWithRowNum> SpecialtyWithRowNum { get; set; }
        public IList<GroupIdea> GroupIdeas { get; set; }
        public IList<FinalGroup> FinalGroups { get; set; }
        public IList<Student> Students { get; set; }
        public IList<Supervisor> Supervisors { get; set; }
    }
}
