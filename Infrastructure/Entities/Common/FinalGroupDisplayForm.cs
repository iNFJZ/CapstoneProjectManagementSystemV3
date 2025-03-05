using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Common
{
    public class FinalGroupDisplayForm : CommonProperty
    {
        public int FinalGroupID { get; set; }
        public string GroupName { get; set; }
        public string ProjectEnglishName { get; set; }
        public int MaxMember { get; set; }
        public int NumberOfMember { get; set; }
        public string SpecialtyFullName { get; set; }
    }
}
