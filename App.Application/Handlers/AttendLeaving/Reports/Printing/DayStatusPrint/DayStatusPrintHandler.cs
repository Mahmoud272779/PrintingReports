using App.Application.Services.Printing;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Domain.Models.Response.HR.AttendLeaving.Reports;
using App.Infrastructure.settings;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using FastReport.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.DayStatusPrint
{
    public class DayStatusPrintHandler : IRequestHandler<DayStatusPrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public DayStatusPrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }

        public async Task<PrintResponseDTO> Handle(DayStatusPrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<DayStatues_Days_Response>)_mediator.Send(request.Report).Result.Data;
            var mainData = new DayStatus_ReportData
            {
                dateFrom = request.Report.dateFrom.ToString(defultData.datetimeFormat),
                dateTo = request.Report.dateTo.ToString(defultData.datetimeFormat)
            };
            var MainData = (mainData, "DayStatus_ReportData");


            var days = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "DayStatues_Days_Response" };
            var Branches = new GetDatatableDTO { obj = data.SelectMany(c => c.DayStatues_Branches).Cast<object>().ToList(), tableName = "DayStatues_Branches" };
            var employees = new GetDatatableDTO { obj = data.SelectMany(c => c.DayStatues_Branches).SelectMany(c => c.DayStatues_employees).Cast<object>().ToList(), tableName = "DayStatues_employees" };

            var lists = new List<GetDatatableDTO>
            {
                days,
                Branches,
                employees
            };
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.DayStatusReport,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType

            };


            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "DayStatus", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes
            };
        }
        public class DayStatus_ReportData
        {
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
        }
    }
}
