using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel.SupervisorViewModel
{
    public class ConfirmFinalGroupResponse
    {
        public string ProjectName { get; set; }

        public string GroupName { get; set; }

        public Profession Profession { get; set; }

        public Specialty Specialty { get; set; }
    }
}
