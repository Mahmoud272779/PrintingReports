using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving
{

    public class GetNormalShiftDaysRsponseDTO  : TimesComman
    {
        public int Id { get; set; } // Id of single detail 
        public bool IsVacation { get; set; }
        public int DayId { get; set; } // from 1 to 7 ids for week days  
        public TimeSpan TotalDayHours { get; set; }
    }
}
