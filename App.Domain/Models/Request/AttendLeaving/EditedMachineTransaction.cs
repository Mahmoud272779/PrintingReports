using App.Domain.Models.Security.Authentication.Request;
using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace App.Domain.Models.Request.AttendLeaving
{

    public class EditAndAddMachineTransactionCommanDTO 
    {
        [Required]
        public DateTime day { get; set; }
        [Required]
        public int machineId { get; set; }
        [Required]
        public TimeSpan transactionTime { get; set; }
        [Required]
        public string password { get; set; }
    }

    public class AddEditedMachineTransactionDTO : EditAndAddMachineTransactionCommanDTO
    {
        [Required]
        public int empId { get; set; }
       
    }
    public class EditEditedMachineTransactionDTO : EditAndAddMachineTransactionCommanDTO
    {
        [Required]
        public int Id { get; set; }
    }
    public class DeleteEditedMachineTransactionDTO
    {
        [Required]
        public string Ids { get; set; }
    }
    public class GetEditedMachineTransactionDTO : PaginationVM
    {
        public string? empId { get; set; }
        public string? branchIds { get; set; }
        public string sectionId { get; set; } = "0";
        public string departmentId { get; set; } = "0";
        public int? shiftId { get; set; }
        [Required]
        public DateTime dateFrom { get; set; }
        [Required]
        public DateTime dateTo { get; set; }
    }
}
