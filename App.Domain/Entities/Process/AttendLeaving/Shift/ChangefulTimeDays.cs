using App.Domain.Entities.Process.AttendLeaving.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class ChangefulTimeDays : TimesComman
    {
        public int Id { get; set; }
        public bool IsRamadan { get; set; }
        public bool IsVacation { get; set; }
        public int changefulTimeGroupsId { get; set; }
        public DateTime day { get; set; }
        public ChangefulTimeGroupsMaster changefulTimeGroups { get; set; }
    }
}
