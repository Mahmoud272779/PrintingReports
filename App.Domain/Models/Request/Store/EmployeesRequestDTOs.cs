using App.Domain.Models.Security.Authentication.Response;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace App.Domain.Models.Security.Authentication.Request
{
    public class EmployeesRequestDTOs
    {
        public class Add
        {
            //Employee Definition
            [Required]
            public int? Code { get; set; }
            [Required]
            public int Status { get; set; }
            [Required]
            public string ArabicName { get; set; }
            public string LatinName { get; set; }
            [Required]
            public int JobId { get; set; }
            public IFormFile? Image { get; set; }
            public string Notes { get; set; }
            [Required]
            public string branches { get; set; }


            //Hiring Information.
            
            public int? gLBranchId { get; set; }
            [Required]
            public int shiftsMasterId { get; set; }
            public int? groupId { get; set; }
            public int? shiftType { get; set; }
            public int? SectionsId { get; set; }
            public int? DepartmentsId { get; set; }
            public int? employeesGroupId { get; set; }
            public int? ManagerId { get; set; }
            public int? projectsId { get; set; }
            public int? missionsId { get; set; }



            //General Leadger
            
            public int? FinancialAccountId { get; set; }


            //Person Information
            public int? nationalityId { get; set; }
            public string? IDNumber { get; set; }
            public int? religionsId { get; set; }
            public DateTime? birthday { get; set; }
            public string? phone { get; set; }
            public string? email { get; set; }

            public string? address { get; set; }


            //Attend Leaving Settings 
            public bool Deduction_of_delay_from_additional_time { get; set; } = false;
            public bool Calculating_extra_time_before_work { get; set; } = false;
            public bool Calculating_extra_time_after_work { get; set; } = false;
            public bool Adding_working_hours_on_vacations { get; set; } = false;
            public bool Auto_Dismissal_registration { get; set; } = false;
        }
        public class Update : Add
        {
            public int Id { get; set; }
        }
        public class Search : GeneralPageSizeParameter
        {
            public string Name { get; set; }
            public int Status { get; set; }
            public int[] JobList { get; set; }
            public int[] BranchList { get; set; }
            public int EmployeeId { get; set; }
            public string DepartmentList { get; set; }
            public string SectiontList { get; set; }
            public string ShiftList { get; set; }
            public string? machineName { get; set; }
            /// <summary>
            /// Only For Print 
            /// </summary>
            public string Ids { get; set; }
            public bool IsSearcheData { get; set; } = true;

        }
        public class NonRegisteredEmployeesDTO : GeneralPageSizeParameter
        {
            public string? searchCriteria { get; set; }
            public string? machineName { get; set; }
        }
    }
}
