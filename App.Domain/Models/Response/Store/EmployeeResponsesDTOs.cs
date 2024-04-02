using App.Domain.Entities;
using App.Domain.Entities.Process;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Models.Security.Authentication.Request;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace App.Domain.Models.Security.Authentication.Response.Store
{
    public class EmployeeResponsesDTOs
    {
        public class GetAll
        {
            public int Id { get; set; }
            public int shiftsMasterId { get; set; }
            public int Code { get; set; }
            public string ArabicName { get; set; }
            public string LatinName { get; set; }
            public int Status { get; set; }
            public string Notes { get; set; }
            public string ImagePath { get; set; }
            public int[] Branches { get; set; }
            public string BranchNameAr { get; set; }
            public string BranchNameEn { get; set; }
            public int JobId { get; set; }
            public string JobNameAr { get; set; }
            public string JobNameEn { get; set; }
            public int JobStatus { get; set; }
            public bool CanDelete { get; set; }
            public int? FinancialAccountId { get; set; }

            //for print 
            public string StatusAr { get; set; }
            public string StatusEn { get; set; }

            public GLBranch GLBranch { get; set; }
            
            public Nationality nationality { get; set; }

            public ShiftsMaster shiftsMaster { get; set; }

            public Projects projects { get; set; }

            public Missions missions { get; set; }

            public SectionsAndDepartments Sections { set; get; }
            public SectionsAndDepartments Departments { set; get; }
            public Machines FirstLogmachine { get; set; }

            public religions religions { set; get; }
            
            public EmployeesGroup employeesGroup { set; get; }
            public int? ManagerId { get; set; }

            public DateTime? birthday { get; set; }
            public string? phone { get; set; }
            public string? email { get; set; }
            public string? address { get; set; }
            public string? IDNumber { get; set; }


            public bool Deduction_of_delay_from_additional_time { get; set; } = false;
            public bool Calculating_extra_time_before_work { get; set; } = false;
            public bool Calculating_extra_time_after_work { get; set; } = false;
            public bool Adding_working_hours_on_vacations { get; set; } = false;
            public bool Auto_Dismissal_registration { get; set; } = false;

        }

        public class GLBranchDTO {

            public int? Id { get; set; } 
            public string? ArabicName { get; set; } 
            public string? LatinName { get; set; } 
            
        }
        public class FA
        {
            public int Id { get; set; }
            public string LatinName { get; set; }
            public string ArabicName { get; set; }
        }

        public class EmployeeDto
        {

            public int EmploeeId { get; set; }
            public string ArabicName { get; set; }
            public string LatinName { get; set; }
            public int Status { get; set; }
            public string Notes { get; set; }
            public string branchNameAr { get; set; }
            public string branchNameEn { get; set; }
            public string jobNameAr { get; set; }
            public string jobNameEn { get; set; }
        }






        /********************************EmployeeDefinition**************************************************************/
        public class GetEmployeeByIdResponseDTO_EmployeeDefinition
        {
            public int Id { get; set; }
            public int Code { get; set; }
            public int Status { get; set; }
            public string ArabicName { get; set; }
            public string LatinName { get; set; }
            public string ImageURL { get; set; }
            public string Notes { get; set; }
            public EmployeeDefinition_Job Job { get; set; }
            public EmployeeDefinition_FinancialAccoun FinancialAccount { get; set; }

        }
        public class EmployeeDefinition_FinancialAccoun
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }
        }
        public class EmployeeDefinition_Job
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }
        }
        /********************************EmployeeDefinition**************************************************************/



        /********************************HiringInformation**************************************************************/
        public class GetEmployeeByIdResponseDTO_HiringInformation
        {
            public HiringInformation_Branch branch { get; set; }
            public HiringInformation_shift shift { get; set; }
            public HiringInformation_Section Section { get; set; }
            public HiringInformation_Department Department { get; set; }
            public HiringInformation_employeesGroup employeesGroup { get; set; }
            public HiringInformation_Manager Manager { get; set; }
            public HiringInformation_project projects { get; set; }
            public HiringInformation_missions missions { get; set; }
        }
        public class HiringInformation_Branch
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

        }
        public class HiringInformation_shift
        {
            public int Id { get; set; }
            public int groupId { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }
            public int shiftType { get; set; }

        }
        public class HiringInformation_Section
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

        }
        public class HiringInformation_Department
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

        }
        public class HiringInformation_employeesGroup
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

        }
        public class HiringInformation_Manager
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

        }
        public class HiringInformation_project
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

        }
        public class HiringInformation_missions
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }

        }
        /********************************HiringInformation**************************************************************/


        /********************************PersonInformation**************************************************************/
        public class GetEmployeeByIdResponseDTO_PersonInformation
        {
            public PersonInformation_nationality nationality { get; set; }
            public PersonInformation_religions religions { get; set; }
            public string? IDNumber { get; set; }
            public DateTime? birthday { get; set; }
            public string? phone { get; set; }
            public string? email { get; set; }
        }
        public class PersonInformation_nationality
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }
        }
        public class PersonInformation_religions
        {
            public int Id { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }
        }
        /********************************PersonInformation**************************************************************/




        public class GetEmployeeByIdResponseDTO_AttendLeavingSettings
        {
            public bool Deduction_of_delay_from_additional_time { get; set; } 
            public bool Calculating_extra_time_before_work { get; set; } 
            public bool Calculating_extra_time_after_work { get; set; } 
            public bool Adding_working_hours_on_vacations { get; set; } 
            public bool Auto_Dismissal_registration { get; set; } 
        }





        public class GetEmployeeByIdResponseDTO
        {
            public GetEmployeeByIdResponseDTO_EmployeeDefinition EmployeeDefinition { get; set; }
            public GetEmployeeByIdResponseDTO_HiringInformation HiringInformation { get; set; }
            public GetEmployeeByIdResponseDTO_PersonInformation PersonInformation { get; set; }
            public GetEmployeeByIdResponseDTO_AttendLeavingSettings AttendLeavingSettings { get; set; }
        }


        /********************************UnRegEmployees**************************************************************/
        public class GetNonRegisteredEmployeesResponseDTO
        {
            public GetNonRegisteredEmployeesResponseDTO_Employee Employee { get; set; }
            public GetNonRegisteredEmployeesResponseDTO_Machine Machine { get; set; }
            public bool canDelete { get; set; } = true;

        }
        public class GetNonRegisteredEmployeesResponseDTO_Employee
        {
            public int Id { get; set; }
            public int code { get; set; }
            public string arabicName { get; set; }
            public string latinName { get; set; }
        }
        public class GetNonRegisteredEmployeesResponseDTO_Machine
        {
            public int? Id { get; set; }
            public string? arabicName { get; set; }
            public string? latinName { get; set; }
        }
        /********************************UnRegEmployees**************************************************************/

    }
}
