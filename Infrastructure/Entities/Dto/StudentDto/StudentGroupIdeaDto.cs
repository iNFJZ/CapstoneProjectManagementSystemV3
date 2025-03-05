using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Dto.StudentDto
{
    public class StudentGroupIdeaDto : CommonProperty
    {
        public string StudentId { get; set; }
        public int GroupIdeaId { get; set; }
        public string Avatar {  get; set; }
        public string FptEmail { get; set; }
        public string RollNumber { get; set; }
        public string FullName { get; set; }
        public string ProfessionFullName { get; set; }
        public string SpecialtyFullName {  get; set; }
        public string CodeOfGroupName {  get; set; }


        //Status:   1 -> Leader
        //          2 -> Member
        //          3 -> Request
        //          4 -> Request accepted / Invited
        //          5 -> Request denied
        //          6 -> leaved group
        public int Status { get; set; }
        public Student Student { get; set; }
        public GroupIdea GroupIdea { get; set; }
        public string Message { get; set; }
    }
}
