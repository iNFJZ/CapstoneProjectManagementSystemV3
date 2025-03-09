using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class NewsDto : CommonProperty
    {
        public int NewID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Pin { get; set; }

        public bool TypeSupport { get; set; }

        public string FileName { get; set; }
        public StaffDto Staff { get; set; }

        public IFormFile AttachedFile { get; set; }
    }
}
