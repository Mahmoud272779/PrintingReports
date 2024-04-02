using App.Domain.Entities.Process.AttendLeaving.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Entities.Process.AttendLeaving
{
    public class ChangefulTimeGroupsMaster
    {
        public int Id { get; set; } 
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public DateTime cDate { get; set; }
        public string Note { get; set; }
        public bool isRamadan { get; set; }
        public int shiftsMasterId { get; set; }
        public DateTime startDate { get; set; }
        public ShiftsMaster shiftsMaster { get; set; }
        public ICollection<ChangefulTimeGroupsDetalies> changefulTimeGroups { get; set; }
        public ICollection<ChangefulTimeDays> changefulTimeDays { get; set; }
        public ICollection<ChangefulTimeGroupsEmployees> changefulTimeGroupsEmployees { get; set; }
    }
}
