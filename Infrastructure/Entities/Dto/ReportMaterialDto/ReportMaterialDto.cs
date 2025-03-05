using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class ReportMaterialDto : CommonProperty
    {
        public int ReportID { get; set; }
        public string ReportTitle { get; set; }
        public string ReportContent { get; set; }
        public int Status { get; set; }
        public DateTime DueDate { get; set; }
        public string SubmissionComment { get; set; }
        public string SubmissionAttachment { get; set; }
        public FinalGroupDto FinalGroup { get; set; }
        public SupervisorDto Supervisor { get; set; }
    }
}
