using App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.AbsancePrint
{
    public class AbsancePrintRequest : CommanPrintingDTO, IRequest<PrintResponseDTO>
    {
        public GetAbsanceReportRequest Report { get; set; }
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
