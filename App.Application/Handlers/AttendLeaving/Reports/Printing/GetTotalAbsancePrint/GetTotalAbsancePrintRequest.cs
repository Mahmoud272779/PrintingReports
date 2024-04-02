using App.Application.Handlers.AttendLeaving.Reports.GetTotalAbsanceReport;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.GetTotalAbsancePrint
{
    public class GetTotalAbsancePrintRequest : CommanPrintingDTO,IRequest<PrintResponseDTO>
    {
        public GetTotalAbsanceReportRequest Report { get; set; }
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
