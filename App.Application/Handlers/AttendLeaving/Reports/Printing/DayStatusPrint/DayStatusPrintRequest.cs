using App.Application.Handlers.AttendLeaving.Reports.DayStatus;
using App.Domain.Models.Request.AttendLeaving.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.DayStatusPrint
{
    public class DayStatusPrintRequest : CommanPrintingDTO, IRequest<PrintResponseDTO>
    {
        public DayStatusRequest Report { get; set; }
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
    }
}
