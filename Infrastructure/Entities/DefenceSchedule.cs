using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class DefenceSchedule
    {
        public int DefenceScheduleId { get; set; }
        public int? Type { get; set; }
        public DateTime? DateDefenceschedule { get; set; }
        public TimeSpan? TimeDefenceschedule { get; set; }
        public string? RoomDefenceschedule { get; set; }
        public string? ConcilInfor { get; set; }
        public int FinalGroupId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual FinalGroup FinalGroup { get; set; } = null!;
    }
}
