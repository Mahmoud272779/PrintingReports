using App.Application.Handlers.AttendLeaving.Reports.TotalAttendance;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.TotalVecationsPrint
{
    public class TotalAttendancePrintRequest : CommanPrintingDTO,IRequest<PrintResponseDTO>
    {
        public TotalAttendanceRequest Report { get; set; }
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
