using App.Application.Handlers.AttendLeaving.Reports.Printing.DetaliedAttendancePrint;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Domain.Models.Response.General;
using App.Domain.Models.Response.HR.AttendLeaving.Reports;
using App.Infrastructure.settings;
using App.Infrastructure.UserManagementDB;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static App.Application.Handlers.AttendLeaving.Reports.Printing.DayStatusPrint.DayStatusPrintHandler;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.vecationsPrint
{
    public class vecationsPrintHandler : IRequestHandler<vecationsPrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public vecationsPrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }
        public async Task<PrintResponseDTO> Handle(vecationsPrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<vecationsReportResponseDTO>)_mediator.Send(request.Report).Result.Data;
            var mainData = new DayStatus_ReportData
            {
                dateFrom = request.Report.dateFrom.ToString(defultData.datetimeFormat),
                dateTo = request.Report.dateTo.ToString(defultData.datetimeFormat)
            };
            var MainData = (mainData, "MainData");

            var Branches = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "Branches" };
            var Employees = new GetDatatableDTO { obj = data.SelectMany(c=> c.employees).Cast<object>().ToList(), tableName = "Employees" };
            var Days = new GetDatatableDTO { obj = data.SelectMany(c=> c.employees).SelectMany(c=> c.days).Cast<object>().ToList(), tableName = "Days" };

            if (Days.obj.Count() <= 0)
            {
                var exRes= new ResponseResult
                {
                    Result = Result.Failed
                };
                return new PrintResponseDTO
                {
                    result = exRes,
                };


            }


            var lists = new List<GetDatatableDTO>
            {
                Branches,
                Days,
                Employees
            };
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.vecationsReport,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType

            };


            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "TotalVecations", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes
            };
        }
    }
    public class vecationsPrint_ReportData
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
    }
}
