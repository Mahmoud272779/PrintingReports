using App.Application.Handlers.AttendLeaving.Reports.DetailedAttendance;
using App.Domain.Models.Request.AttendLeaving.Reports;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.DetaliedAttendancePrint
{
    public class DetaliedAttendancePrintRequest : CommanPrintingDTO, IRequest<PrintResponseDTO>
    {
        public bool isArabic { get; set; }
        public int? fileId { get; set; }
        public exportType exportType { get; set; }
        public DetailedAttendanceRequest Report { get; set; }

    }
}
