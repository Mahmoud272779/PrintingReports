using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Response.HR.AttendLeaving
{
    public class GetMasterShift_ResponseDTO
    {
        public int Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public DateTime dayEndTime { get; set; }
        public int shiftType { get; set; }
        public string shiftTypeNameAr { get; set; }
        public string shiftTypeNameEn { get; set; }
        public bool candelete { get; set; }
    }
    public class GetShiftMasterDropDownlist_ResponseDTO
    {
        public int code { get; set; }
        public int? Id { get; set; }
        public string arabicName { get; set; }
        public string latinName { get; set; }
    }
}
