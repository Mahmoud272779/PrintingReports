using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving
{
    public class GetSectionsAndDepartments
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public Manager manager { get; set; }
        public string ManagerAr { get;set; }
        public string ManagerEn { get;set; }
        public int employeeCount { get;set; }
        public int sectionsCount { get;set; }
        public bool CanDelete { get; set; }
    }
    public class Manager
    {
        public int Id { get; set; }
        public string ArabicName { get; set; }
        public string LatinName { get; set; }
    }

    public class GetSectionsAndDepartmentsDropDownList
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}
