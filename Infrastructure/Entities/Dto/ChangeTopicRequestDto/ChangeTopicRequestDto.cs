using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class ChangeTopicRequestDto : CommonProperty
    {
        public int RequestID { get; set; }
        public string OldTopicNameEnglish { get; set; }
        public string OldTopicNameVietNamese { get; set; }
        public string OldAbbreviation { get; set; }
        public string NewTopicNameEnglish { get; set; }
        public string NewTopicNameVietNamese { get; set; }
        public string NewAbbreviation { get; set; }
        public string EmailSuperVisor { get; set; }
        public string ReasonChangeTopic { get; set; }
        public string StaffComment { get; set; }
        public FinalGroupDto FinalGroup { get; set; }
        public int Status { get; set; }
        /* 
         -1 : reject
         0 : mentor pending
         2 : devhead pending
         3 : staff pending
         4 : accepted
         */

    }
}
