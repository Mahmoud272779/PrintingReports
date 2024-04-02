using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.GetTotalLate;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.TotalLatePrint
{
    public class TotalLatePrintRequest : CommanPrintingDTO, IRequest<PrintResponseDTO>
    {
        public GetTotalLateRequest Report { get; set; } 


        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }

    }


}
