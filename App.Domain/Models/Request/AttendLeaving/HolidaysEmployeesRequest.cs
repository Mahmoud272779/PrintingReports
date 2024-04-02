using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddHolidaysEmployees
    {
        public string EmployeeID { get; set; }
        public int HolidayID { get; set;}
    }

    public class DeleteHolidaysEmployees
    {
        public string EmployeesIds { get; set; }
    }

    public class GetHolidaysEmployees : PaginationVM
    {
        public string? SearchCriteria { get; set; }
        [Required]
        public int holidayId { get; set; }

        public string branchesIds { get; set; }

        public string jobsIds { get; set; }
    }
    public class GetEmployeesForAddModelDTO : PaginationVM
    {
        public int? empId { get; set; }

        public int? code { get; set; }
        public string? branchId { get;}
        public int? SectionsId { get;}
        public int? DepartmentId { get;}
        public int? ShiftId { get;}
        public int? JobId { get;}
        [Required]
        public int holidayId { get; set; }
    }
}
