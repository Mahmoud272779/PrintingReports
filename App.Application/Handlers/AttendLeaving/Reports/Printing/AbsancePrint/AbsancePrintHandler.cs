using App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport;
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

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.AbsancePrint
{
    public class AbsancePrintHandler : IRequestHandler<AbsancePrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public AbsancePrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }
        public async Task<PrintResponseDTO> Handle(AbsancePrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<DayDTO>)_mediator.Send(request.Report).Result.Data;
            var mainData = new Absance_MainData
            {
                dateFrom = request.Report.dateFrom.ToString(defultData.datetimeFormat),
                dateTo = request.Report.dateTo.ToString(defultData.datetimeFormat)
            };
            var MainData = (mainData, "MainData");


            var days = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "Days" };
            var Branches = new GetDatatableDTO { obj = data.SelectMany(c => c.Branches).Cast<object>().ToList(), tableName = "Branches" };
            var employees = new GetDatatableDTO { obj = data.SelectMany(c => c.Branches).SelectMany(c => c.Employees).Cast<object>().ToList(), tableName = "Employees" };
            if (!employees.obj.Any())
            {
                var exRes =  new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "لا يوجد بيانات للطباهه",
                        MessageEn = "There No data to print"
                    }
                };
                return new PrintResponseDTO
                {
                    result = exRes
                };

            }
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
                screenId = (int)SubFormsIds.AbsanceReport,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType

            };


            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "Absance", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes
            };
        }
        public class Absance_MainData
        {
            public string dateFrom { get; set; }
            public string dateTo { get; set; }
        }
    }
}
