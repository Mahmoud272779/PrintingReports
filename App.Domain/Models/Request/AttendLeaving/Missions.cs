using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddMissions
    {
        [Required]
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class EditMissions : AddMissions
    {
        [Required]
        public int Id { get; set; }
    }
    public class GetMissions:PaginationVM
    {
        public string? SearchCriteria { get; set; }
    }
    public class DeleteMissions
    {
        [Required]
        public string Ids { get; set; }
    }
}
