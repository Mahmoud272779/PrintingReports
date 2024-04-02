using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddEmployeeGroups
    {
        [Required]
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
    public class EditEmployeeGroups : AddEmployeeGroups
    {
        [Required]
        public int Id { get; set; }
    }
    public class GetEmployeeGroups : PaginationVM
    {
        public string? SearchCriteria { get; set; }
    }
    public class DeleteEmployeeGroups
    {
        [Required]
        public string Ids { get; set; }
    }

    public class GetEmpsForAddModel : PaginationVM
    {
        

        public int? code { get; set; }

        public string? name { get; set; }
        public string? branchId { get; set; }
        public int? SectionsId { get; set; }


       
        public int? DepartmentId { get; set; }


        public int? ShiftId { get; set; }


        
        public int? JobId { get; set; }

    }

    public class AddEmployeesInParent 
    {
        [Required]
        
        public int parentId { get; set; }

        public string empIds { get; set; }


    }


    public class GetEmployeesInCertainGroup :PaginationVM
    {
        [Required]
        public int groupId { get; set; }

        public string? SearchCriteria { get; set; }


    }

    public class DeleteEmployeesFromCertainGroup
    {
        [Required]

        public int groupId { get; set; }

        public string empIds { get; set; }

    }
}
