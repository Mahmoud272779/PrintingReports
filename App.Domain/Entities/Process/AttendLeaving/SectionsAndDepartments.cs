using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class SectionsAndDepartments
    {
        public int Id { get; set; }
        public int code { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public int? empId { get; set; }
        public int Type { get; set; }
        public int parentId { get; set; }
        public InvEmployees emp { get; set; }
        public ICollection<InvEmployees> invEmployeesSection { get; set; }
        public ICollection<InvEmployees> invEmployeesDepartment { get; set; }
    }
}
