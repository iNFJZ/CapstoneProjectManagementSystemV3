using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class Supervisor
    {
        public Supervisor()
        {
            FinalGroups = new HashSet<FinalGroup>();
            GroupIdeasOfSupervisors = new HashSet<GroupIdeasOfSupervisor>();
            RegisterdGroupSupervisors = new HashSet<RegisterdGroupSupervisor>();
            ReportMaterials = new HashSet<ReportMaterial>();
            SupervisorProfessions = new HashSet<SupervisorProfession>();
        }

        public string SupervisorId { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? IsActive { get; set; }
        public string? SelfDescription { get; set; }
        public string? FieldSetting { get; set; }
        public string? PersonalEmail { get; set; }
        public string? FeEduEmail { get; set; }

        public virtual User SupervisorNavigation { get; set; } = null!;
        public virtual ICollection<FinalGroup> FinalGroups { get; set; }
        public virtual ICollection<GroupIdeasOfSupervisor> GroupIdeasOfSupervisors { get; set; }
        public virtual ICollection<RegisterdGroupSupervisor> RegisterdGroupSupervisors { get; set; }
        public virtual ICollection<ReportMaterial> ReportMaterials { get; set; }
        public virtual ICollection<SupervisorProfession> SupervisorProfessions { get; set; }
    }
}
