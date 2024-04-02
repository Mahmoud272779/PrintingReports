using App.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class AttendancPermission
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public DateTime Day { get; set; }
        public bool IsMoved { get; set; }
        public int type { get; set; }
        public string? note { get; set; }
        public TimeSpan? totalHoursForOpenShift { get; set; }

        public DateTime? shift1_start { get; set; }
        public DateTime? shift1_end { get; set; }
        public bool isShift1_extended { get; set; }

        public bool haveShift2 { get; set; }
        public DateTime? shift2_start { get; set; }
        public DateTime? shift2_end { get; set; }
        public bool isShift2_extended { get; set; }

        public bool haveShift3 { get; set; }
        public DateTime? shift3_start { get; set; }
        public DateTime? shift3_end { get; set; }
        public bool isShift3_extended { get; set; }

        public bool haveShift4 { get; set; }
        public DateTime? shift4_start { get; set; }
        public DateTime? shift4_end { get; set; }
        public bool isShift4_extended { get; set; }

        public InvEmployees employees { get; set; }
    }

}






