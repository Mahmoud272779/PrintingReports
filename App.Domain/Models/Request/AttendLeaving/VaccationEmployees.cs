using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process;
using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddVaccationEmployees
    {
        public int VaccationId { get; set; }

        public int EmployeeId { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string Note { get; set; }
    }

    public class EditVaccationEmployees : AddVaccationEmployees
    {
        [Required]
        public int Id { get; set; }
    }


    public class GetVaccationEmployees : PaginationVM
    {
        public string? SearchCriteria { get; set; }
    }


    public class DeleteVaccationEmployees
    {
        [Required]
        public string Ids { get; set; }
    }
}
