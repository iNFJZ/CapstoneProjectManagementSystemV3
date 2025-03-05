using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.StaffViewModel
{
    public class SupportWithRowNum
    {
        public int RowNum { get; set; }
        public int SupportID { get; set; }
        public string PhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string SupportMessge { get; set; }
        public string Attachment { get; set; }
        public string Title { get; set; }

        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }  //0: pending
                                         //1: processed
        public string Reply { get; set; }
        public DateTime? Reply_At { get; set; }
        public Student Student { get; set; }
    }
}
