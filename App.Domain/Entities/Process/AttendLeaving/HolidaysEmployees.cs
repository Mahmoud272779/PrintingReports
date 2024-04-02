using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class HolidaysEmployees
    {
        public int Id { get; set; }
        public int HolidaysId { get; set; }

        public int EmployeesId { get; set; }

        public InvEmployees Employees  { get; set; }

        public Holidays Holidays  { get; set; }

    }
}
