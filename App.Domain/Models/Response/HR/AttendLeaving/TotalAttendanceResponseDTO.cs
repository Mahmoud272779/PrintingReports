using System.Security.Principal;
using System.Text.Json.Serialization;

namespace App.Domain.Models.Response.HR.AttendLeaving
{
    public class TotalAttendanceDTO_Branches_root
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public List<TotalAttendanceDTO_Employees> employees { get; set; }
    }
    public class TotalAttendanceDTO_Employees
    {
        public int Id { get; set; }
        public int branchId { get; set; }
        public TotalAttendance_Emp employee { get; set; }
        public int code { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        [JsonIgnore]
        public string jobAr { get; set; }
        [JsonIgnore]
        public string jobEn { get; set; }
        public TotalAttendance_job job { get; set; }
        [JsonIgnore]
        public string shiftAr { get; set; }
        [JsonIgnore]
        public string shiftEn { get; set; }
        public TotalAttendance_Shift shift { get; set; }
        public string totalHours { get; set; }
        public string delay { get; set; }
        public string absence { get; set; }
        public string vacations { get; set; }
        public string extraTime { get; set; }
        public string actualWorkingHours { get; set; }

    }
    public class TotalAttendance_Emp
    {
        public int Id { get; set; }
        public int code { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class TotalAttendance_job
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class TotalAttendance_Shift
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }

}
