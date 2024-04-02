using App.Domain.Entities.Process.AttendLeaving.Shift;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving
{
    public class GetChangefulTimeDaysRsponseDTO
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public HashSet<GetChangefulTimeDaysDTO_Detalies> detlies { get; set; }
        public HashSet<GetChangefulTimeDaysDTO_Days> days { get; set; }
    }

    public class GetChangefulTimeDaysDTO_Comman : TimesComman
    {
        public int Id { get; set; }
        public int changefulTimeGroupsId { get; set; }
    }

    public class GetChangefulTimeDaysDTO_Detalies : GetChangefulTimeDaysDTO_Comman
    {
        public int workDaysNumber { get; set; }
        public int weekendNumber { get; set; }
    }
    public class GetChangefulTimeDaysDTO_Days : GetChangefulTimeDaysDTO_Comman
    {
        public bool IsVacation { get; set; }
        
    }
}
