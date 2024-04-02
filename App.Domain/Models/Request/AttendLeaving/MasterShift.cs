using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static App.Domain.Enums.Enums;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddMasterShift
    {
        [Required]
        public string arabicName { get; set; }
        public string latinName { get; set; }
        public DateTime? dayEndTime { get; set; }
        public shiftTypes shiftType { get; set; }
    }
    public class EditMasterShift : AddMasterShift
    {
        [Required]
        public int Id { get; set; }
    }
    public class GetMasterShift : PaginationVM
    {
        public string? searchCriteria { get; set; }
        public string? name { get; set; }
    }
    public class DeleteMasterShift 
    {
        [Required]
        public string Ids { get; set; }
    }
    public class EditNormalShiftDaysDTO : TimesComman
    {
        public bool IsVacation { get; set; }
        public int shiftId { get; set; }
        public int DayId { get; set; }
        public bool IsRamadan { get; set; }
        public bool repeteAll { get; set; }
        public string? totalHoursForOpenShift { get; set; }

        
    }
    public class EditOpenShiftDaysDTO
    {
        public bool IsVacation { get; set; }
        public int ShiftId { get; set; }
        public int DayId { get; set; }
        public bool IsRamadan { get; set; }
        public bool repeteAll { get; set; }
        public bool totalShiftHours { get; set; }
    }
    public class GetNormalShiftDaysRequest
    {
        public int ShiftId { get; set; }
        public bool? isRamadan { get; set; } = false;
    }
}
