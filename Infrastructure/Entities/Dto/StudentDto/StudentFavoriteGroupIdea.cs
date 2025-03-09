using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class StudentFavoriteGroupIdeaDto : CommonProperty
    {
        public string StudentID { get; set; }
        public int GroupIdeaID { get; set; }
        public StudentDto Student { get; set; }
        public GroupIdeaDto GroupIdea { get; set; }
    }
}
