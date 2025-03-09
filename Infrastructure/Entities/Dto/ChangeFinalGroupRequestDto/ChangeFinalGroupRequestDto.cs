using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class ChangeFinalGroupRequestDto : CommonProperty
    {
        public int ChangeFinalGroupRequestId { get; set; }
        public StudentDto FromStudent { get; set; }
        public StudentDto ToStudent { get; set; }
        public int StatusOfToStudent { get; set; }
        public int StatusOfStaff { get; set; }
        public string StaffComment { get; set; }
    }
}
