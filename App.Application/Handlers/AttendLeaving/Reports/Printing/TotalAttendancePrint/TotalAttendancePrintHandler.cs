using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Domain.Models.Response.HR.AttendLeaving;
using App.Domain.Models.Response.HR.AttendLeaving.Reports;
using App.Infrastructure.settings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static App.Application.Handlers.AttendLeaving.Reports.Printing.DayStatusPrint.DayStatusPrintHandler;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.TotalVecationsPrint
{
    public class TotalVecationsPrintHandler : IRequestHandler<TotalAttendancePrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public TotalVecationsPrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }
        public async Task<PrintResponseDTO> Handle(TotalAttendancePrintRequest request, CancellationToken cancellationToken)
        {
            var report = await _mediator.Send(request.Report);
            var data = (List<TotalAttendanceDTO_Branches_root>)report.Data;
            var mainData = new TotalAttendancePrint_ReportData
            {
                dateFrom = request.Report.dateFrom.ToString(defultData.datetimeFormat),
                dateTo = request.Report.dateTo.ToString(defultData.datetimeFormat)
            };
            var MainData = (mainData, "MainData");


            var Branches = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "Branches" };
            var employees = new GetDatatableDTO { obj = data.SelectMany(c => c.employees).Cast<object>().ToList(), tableName = "Employees" };

            var lists = new List<GetDatatableDTO>
            {
                Branches,
                employees
            };
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.TotalAttendance,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType

            };


            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "TotalAttendance", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes
            };
        }

        public class TotalAttendancePrint_ReportData
        {
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
        }
    }
}
