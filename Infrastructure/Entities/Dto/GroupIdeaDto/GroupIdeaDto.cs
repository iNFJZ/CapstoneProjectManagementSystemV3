using Infrastructure.Custom;
using Infrastructure.Entities.Dto.RegisteredDto;
using Infrastructure.Entities.Dto.SemesterDto;
using Infrastructure.Entities.Dto.SpecialtyDto;
using Infrastructure.Entities.Dto.StudentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class GroupIdeaDto : CommonProperty
    {
        public int GroupIdeaID { get; set; }
        public string ProjectEnglishName { get; set; }
        public string ProjectVietNameseName { get; set; }
        public string Abrrevation { get; set; }
        public string Description { get; set; }
        public string ProjectTags { get; set; }
        public int NumberOfMember { get; set; }
        public int MaxMember { get; set; }
        public int ProfessionId {  get; set; }
        public int SpecialtyId { get; set; }
        public ProfessionDto Profession { get; set; }
        public SpecialtyDto Specialty { get; set; }
        public SemesterDto Semester { get; set; }
        public bool? IsActive { get; set; }
        public IList<RegisteredGroupDto> RegisteredGroups { get; set; }
        public IList<StudentGroupIdeaDto> StudentGroupIdeas { get; set; }
        public IList<StudentDto> Students { get; set; }
    }
}
