using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving.Reports
{
    public class DayStatues_employees
    {
        public int code { get; set; }
        public string day { get; set; }
        public int branchId { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public string shiftAr { get; set; }
        public string shiftEn { get; set; }
        public string? shift1_TimeIn { get; set; }
        public string? shift1_TimeOut { get; set; }
        public string? shift2_TimeIn { get; set; }
        public string? shift2_TimeOut { get; set; }
        public string? shift3_TimeIn { get; set; }
        public string? shift3_TimeOut { get; set; }
        public string? shift4_TimeIn { get; set; }
        public string? shift4_TimeOut { get; set; }

        public string? totalShiftTime { get; set; }
        public string? lateTime { get; set; }
        public string? ExtraTime { get; set; }
        public string? WorkTime { get; set; }
        public string  dayStatusAr { get; set; }
        public string  dayStatusEn { get; set; }
    }
    public class DayStatues_Branches
    {
        public List<DayStatues_employees> DayStatues_employees { get; set; }
        public int branchId { get; set; }
        public string day { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public bool expanded { get; set; }

    }
    public class DayStatues_Days_Response
    {
        public List<DayStatues_Branches> DayStatues_Branches { get; set; }
        public string dayAr { get; set; }
        public string dayEn { get; set; }
        public string day { get; set; }
        public bool expanded { get; set; }
    }
}
