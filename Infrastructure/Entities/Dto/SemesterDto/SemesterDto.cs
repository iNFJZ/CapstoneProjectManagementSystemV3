using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class SemesterDto : CommonProperty
    {
        public int SemesterID { get; set; }
        public string SemesterName { get; set; }
        public string SemesterCode { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? DeadlineChangeIdea { get; set; }

        public DateTime? DeadlineRegisterGroup { get; set; }
        public bool? IsConfirmationOfDevHeadNeeded { get; set; }
        public bool StatusClose { get; set; }
        public bool? ShowGroupName { get; set; }
        public IList<GroupIdeaDto> GroupIdeas { get; set; }
        public IList<FinalGroupDto> FinalGroups { get; set; }

        public string? SubjectMailTemplate { get; set; }
        public string? BodyMailTemplate { get; set; }
    }
}
