using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving.Reports
{
    public class GetReportRequest
    {
        public int?  EmpId { get; set; }
        public int?  DepartmentId { get; set; }
        public string?  BranchId { get; set; }
        public int?  JobId { get; set; }
        public int?  GroupId { get; set; }
        public int?  ShiftmasterId { get; set; }
        public int?  SectionId { get; set; }


    }


    public class GetExtraReportRequest : GetReportRequest
    {
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }

    }



    public class GetAbsanceReportRequest : GetReportRequest
    {
        [Required]
        public DateTime dateFrom { get; set; }
        [Required]
        public DateTime dateTo { get; set; }

    }

    public class GetTotalAbsanceReportRequest : GetReportRequest
    {
        [Required]
        public DateTime dateFrom { get; set; }
        [Required]
        public DateTime dateTo { get; set; }

    }

    public class GetTotalLateReportRequestDTO : GetReportRequest
    {
        public int? projectId { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }

    }

    public class AttendancePermissionsRequestDTO : GetTotalLateReportRequestDTO
    {
        public string? name { get; set; }
        public int? permissionType { get; set; }
        public int? projectId { get; set; }
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }

    }

    public class GetDetailedLateReportRequestDTO : GetTotalLateReportRequestDTO
    {
      

    }

    public class GetAttendLateLeaveEarlyReportRequestDTO : GetTotalLateReportRequestDTO
    {


    }

}
