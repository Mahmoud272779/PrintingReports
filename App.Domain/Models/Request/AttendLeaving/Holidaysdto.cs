using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddHolidays
    {
        [Required]
        public string arabicName { get; set; }
        
        public string latinName { get; set; }
       
        public DateTime startdate { get; set; }
        public DateTime enddate { get; set; }
    }

    public class EditHolidays : AddHolidays
    {
        [Required]
        public int Id { get; set; }
    }


    public class GetHolidays : PaginationVM
    {
        public string? SearchCriteria { get; set; }
    }


    public class DeleteHolidays
    {
        [Required]
        public string Ids { get; set; }
    }

    public class EditHolidaysEmployees
    {
        [Required]
        public string EmployeesIds { get; set; }

        [Required]
        public int HolidayId { get; set; }
    }
}
