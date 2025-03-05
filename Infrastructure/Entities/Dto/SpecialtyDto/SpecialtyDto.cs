using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.SpecialtyDto
{
    public class SpecialtyDto : CommonProperty
    {
        public int SpecialtyID { get; set; }
        public string SpecialtyFullName { get; set; }
        public string SpecialtyAbbreviation { get; set; }
        public int MaxMember { get; set; }
        public string CodeOfGroupName { get; set; }
        public Semester Semester { get; set; }
        public Profession Profession { get; set; }
        public IList<GroupIdea> GroupIdeas { get; set; }
        public IList<FinalGroup> FinalGroups { get; set; }
        public IList<Student> Students { get; set; }
        public IList<With> listWith { get; set; }
    }
}
