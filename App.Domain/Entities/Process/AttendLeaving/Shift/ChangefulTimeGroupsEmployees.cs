using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving.Shift
{
    public class ChangefulTimeGroupsEmployees
    {
        public int Id { get; set; }
        public int changefulTimeGroupsMasterId { get; set; }
        public int invEmployeesId { get; set; }
        public ChangefulTimeGroupsMaster changefulTimeGroupsMaster { get; set; }
        public InvEmployees invEmployees { get; set; }

    }
}
