using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.StaffViewModel
{
    public class NewsWithRowNum
    {
        public int RowNum { get; set; }
        public int NewID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool? Pin { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? TypeSupport { get; set; }
        public Staff Staff { get; set; }
        public string Filename { get; set; }
        public byte[] AttachedFile { get; set; }
    }
}
