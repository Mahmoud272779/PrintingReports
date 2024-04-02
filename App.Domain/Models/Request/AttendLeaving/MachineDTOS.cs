using App.Domain.Models.Shared;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddMachineDTO
    {
        [Required]
        public string arabicName { get; set; }
        public string latinName { get; set; }
        [Required]
        public string MachineSN { get; set; }
        [Required]
        public int branchId { get; set; }
    }
    public class EditMachineDTO : AddMachineDTO
    {
        public int Id { get; set; }
    }
    public class DeleteMachineDTO
    {
        public string Ids { get; set; }
    }
    public class GetMachineDTO : PaginationVM
    {
        public string? searchCriteria { get; set; }
        public string? branchId { get; set; }
        
    }
}
