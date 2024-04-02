using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.AttendancePermissionsReport;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.AttendancePermissionPrint
{
    public class AttendancePermissionPrintRequest : CommanPrintingDTO, IRequest<ResponseResult>
    {
        public AttendancePermissionsReportRequest Report { get; set; }
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
