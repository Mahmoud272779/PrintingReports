using App.Domain.Entities.Process.AttendLeaving.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class NormalShiftDetalies : TimesComman
    {
        public int Id { get; set;} // Id of single detail 
        public bool IsVacation { get; set;}
        public int ShiftId { get; set; } // shiftID from ShiftsMaster 
        public int DayId { get; set;} // 7 ids for week days
        public TimeSpan TotalDayHours { get; set; }
        public bool IsRamadan { get; set; }
        public ShiftsMaster Shift { get; set;} 
    }
}
