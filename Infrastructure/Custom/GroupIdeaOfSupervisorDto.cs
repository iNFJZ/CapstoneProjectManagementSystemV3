using System;
using System.Collections.Generic;
using Infrastructure.Entities;

namespace Infrastructure.Custom
{
    public class GroupIdeaOfSupervisorDto
    {
        public int GroupIdeaID { get; set; }
        public string ProjectEnglishName { get; set; }
        public string ProjectVietNameseName { get; set; }
        public string Abrrevation { get; set; }
        public string Description { get; set; }
        public string ProjectTags { get; set; }
        public int NumberOfMember { get; set; }
        public int MaxMember { get; set; }
        public string SupervisorID { get; set; }
        public Semester Semester { get; set; }
        public bool? IsActive { get; set; }
        public Specialty Specialty { get; set; }
        public Profession Profession { get; set; }
        public Supervisor Supervisor { get; set; }
        public GroupIdeaOfSupervisorProfession GroupIdeaOfSupervisorProfession { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<GroupIdeaOfSupervisorProfession> GroupIdeaOfSupervisorProfessions { get; set; }

        public IList<RegisteredGroup> RegisteredGroups { get; set; }
    }
}
