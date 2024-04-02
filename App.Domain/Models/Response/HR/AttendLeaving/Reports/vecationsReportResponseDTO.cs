using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving.Reports
{
    public class vecationsReportResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public List<vecationsReport_Employees> employees { get; set; }

    }
    public class vecationsReport_Employees
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int code { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        [JsonIgnore]
        public string jobAr { get; set; }
        [JsonIgnore]
        public string jobEn { get; set; }
        [JsonIgnore]
        public string shiftAr { get; set; }
        [JsonIgnore]
        public string shiftEn { get; set; }
        public Employee_job job { get; set; }
        public Employee_shift shift { get; set; }
        public double totalDays { get; set; }
        public List<vecationsReport_days> days { get; set; }
    }
    public class Employee_job
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class Employee_shift
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class vecationsReport_days
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
        public double totalDays { get; set; }
        public string note { get; set; }
    }
  
}
