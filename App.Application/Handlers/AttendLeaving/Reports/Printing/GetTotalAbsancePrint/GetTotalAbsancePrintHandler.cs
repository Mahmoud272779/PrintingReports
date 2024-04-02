using App.Application.Handlers.AttendLeaving.Reports.GetTotalAbsanceReport;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
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

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.GetTotalAbsancePrint
{
    public class GetTotalAbsancePrintHandler : IRequestHandler<GetTotalAbsancePrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public GetTotalAbsancePrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }

        public async Task<PrintResponseDTO> Handle(GetTotalAbsancePrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<TotalAbsance_Branches>)_mediator.Send(request.Report).Result.Data;
            var mainData = new TotalAbsancePrintMainData
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
                screenId = (int)SubFormsIds.TotalAbsanceReport,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType

            };


            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "TotalAbsance", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes
            };
        }

        public class TotalAbsancePrintMainData
        {
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
        }
    }
}
