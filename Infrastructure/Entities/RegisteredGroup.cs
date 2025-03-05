using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class RegisteredGroup
    {
        public RegisteredGroup()
        {
            RegisterdGroupSupervisors = new HashSet<RegisterdGroupSupervisor>();
        }

        public int RegisteredGroupId { get; set; }
        public int GroupIdeaId { get; set; }
        public string? RegisteredSupervisorName1 { get; set; }
        public string? RegisteredSupervisorName2 { get; set; }
        public string? RegisteredSupervisorPhone1 { get; set; }
        public string? RegisteredSupervisorPhone2 { get; set; }
        public string? RegisteredSupervisorEmail1 { get; set; }
        public string? RegisteredSupervisorEmail2 { get; set; }
        public string? StudentComment { get; set; }
        public int? Status { get; set; }
        public string? StaffComment { get; set; }
        public string? StudentsRegistraiton { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual GroupIdea GroupIdea { get; set; } = null!;
        public virtual ICollection<RegisterdGroupSupervisor> RegisterdGroupSupervisors { get; set; }
    }
}
