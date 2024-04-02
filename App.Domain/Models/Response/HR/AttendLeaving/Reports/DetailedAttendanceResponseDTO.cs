using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving.Reports
{
    public class DetailedAttendanceResponseDTO_Days
    {
        public int branchId { get; set; }
        public int employeeId { get; set; }
        public string dayAr { get; set; }
        public string dayEn { get; set; }
        public string date { get; set; }

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
        public string? extraTime { get; set; }
        public string? workingTime { get; set; }

        public int status { get; set; }
        public string statusAr { get; set; }
        public string statusEn { get; set; }
    }
    public class DetailedAttendanceResponseDTO_Employees
    {
        public List<DetailedAttendanceResponseDTO_Days> days { get; set; }
        public int Id { get; set; }
        public int branchId { get; set; }
        public int code { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public string jobAr { get; set; }
        public string jobEn { get; set; }
        public string shiftAr { get; set; }
        public string shiftEn { get; set; }
        public bool expanded { get; set; }
        public string? TotalLateTime { get; set; }
        public string? TotalExtraTime { get; set; }
        public string? TotalWorkingTime { get; set; }

    }
    public class DetailedAttendanceResponseDTO_Branches
    {
        public List<DetailedAttendanceResponseDTO_Employees> employees { get; set; }
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public bool expanded { get; set; }

    }
}
