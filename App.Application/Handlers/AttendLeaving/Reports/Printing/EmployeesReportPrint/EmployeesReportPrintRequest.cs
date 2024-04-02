using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.GetReport;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.EmployeesReportPrint
{
    public class EmployeesReportPrintRequest : CommanPrintingDTO, IRequest<PrintResponseDTO>
    {
        public GetReportRequest Report { get; set; } = new GetReportRequest();


        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
