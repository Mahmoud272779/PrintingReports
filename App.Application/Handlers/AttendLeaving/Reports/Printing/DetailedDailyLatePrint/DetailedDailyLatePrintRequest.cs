using App.Application.Handlers.AttendLeaving.Reports.DetailedDailyLate;
using MediatR;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.DetailedDailyLatePrint
{
    public class DetailedDailyLatePrintRequest : CommanPrintingDTO, IRequest<PrintResponseDTO>
    {
        public DetailedDailyLateRequest Report { get; set; } = new DetailedDailyLateRequest();
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
