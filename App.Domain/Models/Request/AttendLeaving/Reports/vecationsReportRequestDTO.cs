using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Models.Request.AttendLeaving.Reports
{
    public class vecationsReportRequestDTO
    {
        public int? employeeId { get; set; }
        public string? branches { get; set; }
        public string? managements { get; set; }
        public string? sections { get; set; }
        public int? projectId { get; set; }
        public int? jobId { get; set; }
        public int? groupId { get; set; }
        public int? shiftId { get; set; }
        public int? vecationTypeId { get; set; }
        [Required]
        public DateTime dateFrom { get; set; }
        [Required]
        public DateTime dateTo { get; set; }
    }
}
