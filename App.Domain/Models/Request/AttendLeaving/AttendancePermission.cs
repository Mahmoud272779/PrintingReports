using App.Domain.Entities.Process;
using App.Domain.Models.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving
{
    public class AddAttendancePermission
    {

        public int EmpId { get; set; }
        public DateTime Day { get; set; }
        public int type { get; set; }
        public TimeSpan? totalHoursForOpenShift { get; set; }
        public TimeSpan? shift1_start { get; set; }
        public TimeSpan? shift1_end { get; set; }
        public TimeSpan? shift2_start { get; set; }
        public TimeSpan? shift2_end { get; set; }
        public TimeSpan? shift3_start { get; set; }
        public TimeSpan? shift3_end { get; set; }
        public TimeSpan? shift4_start { get; set; }
        public TimeSpan? shift4_end { get; set; }
        public string? Note { get; set; }
        public bool Has_shift2 { get; set; }
        public bool Has_shift3 { get; set; }
        public bool Has_shift4 { get; set; }
        public bool shift1_IsExtended { get; set; }
        public bool shift2_IsExtended { get; set; }
        public bool shift3_IsExtended { get; set; }
        public bool shift4_IsExtended { get; set; }

    }

    public class EditAttendancePermission : AddAttendancePermission
    {
        [Required]
        public int Id { get; set; }
    }

    public class DeleteAttendancePermission
    {
        [Required]
        public string Ids { get; set; }
    }

    public class GetAttendancePermission : PaginationVM
    {
        public string? SearchCriteria { get; set; }
    }
}
