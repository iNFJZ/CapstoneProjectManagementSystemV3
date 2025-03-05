using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.SupervisorViewModel
{
    public class GroupIdeaOfSupervisorWithRowNum
    {
        public int RowNum { get; set; }
        public int GroupIdeaID { get; set; }
        public string ProjectEnglishName { get; set; }
        public string ProjectVietNameseName { get; set; }
        public string Abrrevation { get; set; }
        public string Description { get; set; }
        public string ProjectTags { get; set; }
        public int NumberOfMember { get; set; }
        public int MaxMember { get; set; }

        public DateTime? CreatedAt { get; set; }

        public Semester Semester { get; set; }
        public bool? IsActive { get; set; }

        public Supervisor Supervisor { get; set; }

        public IList<GroupIdeaOfSupervisorProfession> GroupIdeaOfSupervisorProfessions { get; set; }

        public IList<RegisteredGroup> RegisteredGroups { get; set; }
    }
}
