using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class SpecialtyDto : CommonProperty
    {
        public int SpecialtyID { get; set; }
        public string SpecialtyFullName { get; set; }
        public string SpecialtyAbbreviation { get; set; }
        public int MaxMember { get; set; }
        public string CodeOfGroupName { get; set; }
        public SemesterDto Semester { get; set; }
        public ProfessionDto Profession { get; set; }
        public IList<GroupIdeaDto> GroupIdeas { get; set; }
        public IList<FinalGroupDto> FinalGroups { get; set; }
        public IList<StudentDto> Students { get; set; }
        public IList<WithDto> listWith { get; set; }
    }
}
