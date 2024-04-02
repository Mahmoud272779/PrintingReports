using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation.Models
{
    public class Shift
    {
        public TimeSpan shift1_startIn { get; set; }
        public TimeSpan shift1_endIn { get; set; }
        public TimeSpan shift1_startOut { get; set; }
        public TimeSpan shift1_endOut { get; set; }
        public TimeSpan shift1_Start { get; set; }
        public TimeSpan shift1_End { get; set; }
        public bool shift1_IsExtended { get; set; }
        public TimeSpan shift1_lateBefore { get; set; }
        public TimeSpan shift1_lateAfter { get; set; }

        public TimeSpan shift2_startIn { get; set; }
        public TimeSpan shift2_endIn { get; set; }
        public TimeSpan shift2_startOut { get; set; }
        public TimeSpan shift2_endOut { get; set; }
        public TimeSpan shift2_Start { get; set; }
        public TimeSpan shift2_End { get; set; }
        public bool IsHaveShift2 { get; set; }
        public bool shift2_IsExtended { get; set; }
        public TimeSpan shift2_lateBefore { get; set; }
        public TimeSpan shift2_lateAfter { get; set; }

        public TimeSpan shift3_startIn { get; set; }
        public TimeSpan shift3_endIn { get; set; }
        public TimeSpan shift3_startOut { get; set; }
        public TimeSpan shift3_endOut { get; set; }
        public TimeSpan shift3_Start { get; set; }
        public TimeSpan shift3_End { get; set; }
        public bool IsHaveShift3 { get; set; }
        public bool shift3_IsExtended { get; set; }
        public TimeSpan shift3_lateBefore { get; set; }
        public TimeSpan shift3_lateAfter { get; set; }

        public TimeSpan shift4_startIn { get; set; }
        public TimeSpan shift4_endIn { get; set; }
        public TimeSpan shift4_startOut { get; set; }
        public TimeSpan shift4_endOut { get; set; }
        public TimeSpan shift4_Start { get; set; }
        public TimeSpan shift4_End { get; set; }
        public bool IsHaveShift4 { get; set; }
        public bool shift4_IsExtended { get; set; }
        public TimeSpan shift4_lateBefore { get; set; }
        public TimeSpan shift4_lateAfter { get; set; }

        public bool IsVacation { get; set; }
        public int type { get; set; }
        public TimeSpan totalHours_ForOpenShift { get; set; }
    }
}
