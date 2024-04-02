using App.Application.Handlers.AttendLeaving.Reports.vecationsReport;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.vecationsPrint
{
    public class vecationsPrintRequest : CommanPrintingDTO, IRequest<PrintResponseDTO>
    {
        public vecationsReportRequest Report { get; set; }
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
