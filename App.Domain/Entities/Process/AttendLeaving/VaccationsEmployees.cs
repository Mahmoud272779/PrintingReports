using App.Domain.Entities.Process;
using App.Domain.Entities.Process.AttendLeaving;
using System;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class VaccationEmployees
    {

        public int Id { get; set; }

        public int VaccationId { get; set; }

        public int EmployeeId { get; set; }

        public InvEmployees Employees { get; set; }

        public Vaccation Vaccations { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string Note { get; set; }
    }
}

