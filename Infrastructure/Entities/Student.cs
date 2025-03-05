
using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Student
    {
        public Student()
        {
            ChangeFinalGroupRequestFromStudents = new HashSet<ChangeFinalGroupRequest>();
            ChangeFinalGroupRequestToStudents = new HashSet<ChangeFinalGroupRequest>();
            Supports = new HashSet<Support>();
        }

        public string StudentId { get; set; } = null!;
        public string? RollNumber { get; set; }
        public string? Curriculum { get; set; }
        public string? SelfDiscription { get; set; }
        public string? ExpectedRoleInGroup { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LinkFacebook { get; set; }
        public string? EmailAddress { get; set; }
        public int? SemesterId { get; set; }
        public int? FinalGroupId { get; set; }
        public int? ProfessionId { get; set; }
        public int? SpecialtyId { get; set; }
        public string? GroupName { get; set; }
        public bool? IsLeader { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? WantToBeGrouped { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsEligible { get; set; }

        public virtual FinalGroup? FinalGroup { get; set; }
        public virtual Profession? Profession { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Specialty? Specialty { get; set; }
        public virtual User StudentNavigation { get; set; } = null!;
        public virtual ICollection<ChangeFinalGroupRequest> ChangeFinalGroupRequestFromStudents { get; set; }
        public virtual ICollection<ChangeFinalGroupRequest> ChangeFinalGroupRequestToStudents { get; set; }
        public virtual ICollection<Support> Supports { get; set; }
    }
}
