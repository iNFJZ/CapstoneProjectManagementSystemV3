using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class GroupIdeaDisplayForm
    {
        public int GroupIdeaID { get; set; }
        public string ProjectEnglishName { get; set; }
        public string Description { get; set; }
        public List<string> ProjectTags { get; set; }
        public int AvailableSlot { get; set; }
        public string ProfessionFullName { get; set; }
        public string SpecialtyFullName { get; set; }
        public string LeaderFullName { get; set; }
        public string Avatar { get; set; }
        public string CreatedAt { get; set; }
        public int Semester_Id { get; set; }
    }
}
