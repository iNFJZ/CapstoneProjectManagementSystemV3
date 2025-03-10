﻿using Infrastructure.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class FinalGroupDto : CommonProperty
    {
        public int FinalGroupId { get; set; }
        public string GroupName { get; set; }
        public string? Description { get; set; }
        public int? MaxMember { get; set; }
        public int? NumberOfMember { get; set; }
        public int ProfessionId { get; set; }
        public int SpecialtyId { get; set; }
        public string? ProjectEnglishName { get; set; }
        public string? ProjectVietNameseName { get; set; }
        public string Abbreviation { get; set; } = null!;
        public int SemesterId { get; set; }
        public string? SupervisorId { get; set; }
        public bool? IsConfirmFinalReport { get; set; }
        public string? SupervisorName { get; set; }
        public string? ProfessionFullName { get; set; }
        public string? SpecialtyFullName { get; set; }
        public string? UserName { get; set; }
        public string UserId { get; set; }
        public string SemesterCode { get; set; }

        public virtual ProfessionDto Profession { get; set; } = null!;
        public virtual SemesterDto Semester { get; set; } = null!;
        public virtual SpecialtyDto Specialty { get; set; } = null!;
        public virtual SupervisorDto? Supervisor { get; set; }
    }
}
