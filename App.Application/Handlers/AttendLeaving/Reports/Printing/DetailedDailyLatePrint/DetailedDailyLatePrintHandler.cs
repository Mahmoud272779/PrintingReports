using App.Application.Handlers.AttendLeaving.Reports.DetailedDailyLate;
using App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Infrastructure.settings;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static App.Application.Handlers.AttendLeaving.Reports.Printing.AbsancePrint.AbsancePrintHandler;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.DetailedDailyLatePrint
{
    public class DetailedDailyLatePrintHandler : IRequestHandler<DetailedDailyLatePrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public DetailedDailyLatePrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }
        public async Task<PrintResponseDTO> Handle(DetailedDailyLatePrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<DetailedlateResponseDTO_Branches>)_mediator.Send(request.Report).Result.Data;
            var mainData = new DetailedDailyLate_MainData
            {
                dateFrom = request.Report.DateFrom.ToString(defultData.datetimeFormat),
                dateTo = request.Report.DateTo.ToString(defultData.datetimeFormat)
            };
            var MainData = (mainData, "MainData");


            var Branches = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "Branches" };
            var employees = new GetDatatableDTO { obj = data.SelectMany(c=> c.employees).Cast<object>().ToList(), tableName = "Employees" };
            var days = new GetDatatableDTO { obj = data.SelectMany(c => c.employees).SelectMany(c => c.days).Cast<object>().ToList(), tableName = "Days" };
            if (!days.obj.Any() || !employees.obj.Any() || !Branches.obj.Any())
            {
                var exRes =  new ReportsReponse()
                {
                    Result = Result.Failed,
                };
                return new PrintResponseDTO
                {
                    result = exRes,
                };

            }
            var lists = new List<GetDatatableDTO>
            {
                Branches,
                days,
                employees
            };
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.GetDetailedLateReport,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType

            };
            var webReport = await _iGeneralPrint.PrintReport(printReport);
            var reportRes = await _iPrintFileService.PrintFile(webReport, "Detailedlate", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes,
            }; ;
        }
        public class DetailedDailyLate_MainData
        {
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
        }
    }
}
