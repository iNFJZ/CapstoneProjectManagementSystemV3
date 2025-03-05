using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities.Common.Paging
{
    public class PageRepon<T>
    {
        public int TotalRecord { get; set; }
        public List<T> Items { get; set; }
    }
}
