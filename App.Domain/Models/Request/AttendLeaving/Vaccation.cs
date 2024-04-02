using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddVaccatiuon
    {
        [Required]
        public string arabicName { get; set; }

        public string latinName { get; set; }

        


    }

    public class EditVacction : AddHolidays
    {
        [Required]
        public int Id { get; set; }
    }


    public class GetVacction : PaginationVM
    {
        public string? SearchCriteria { get; set; }
    }


    public class DeleteVaccation
    {
        [Required]
        public string Ids { get; set; }
    }

    
}
