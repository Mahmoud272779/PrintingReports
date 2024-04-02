using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Domain.Models.Shared;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddRamadanDate
    {
        [Required]
        public string arabicName { get; set; }
        public string latinName { get; set; }//rth
        [Required]
        public DateTime startdate { get; set; }
        [Required]
        public DateTime enddate { get; set; }
        public string? Note { get; set; }
    }
    public class EditRamadanDate : AddRamadanDate
    {
        [Required]
        public int Id { get; set; }
    }
    public class GetAllRamadanDates : PaginationVM
    {
        public string? searchCriteria { get; set; }

    }
    public class DeleteRamadanDate
    {
        [Required]
        public string Ids { get; set; }
    }
}
