using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.Printing.DayStatusPrint;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Domain.Models.Response.HR.AttendLeaving.Reports;
using App.Infrastructure.settings;
using MediatR;
using static App.Application.Handlers.AttendLeaving.Reports.Printing.DayStatusPrint.DayStatusPrintHandler;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.AttendLateLeaveEarly
{
    public class AttendLateLeaveEarlyPrintHandler : IRequestHandler<AttendLateLeaveEarlyPrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public AttendLateLeaveEarlyPrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }

       
        public async Task<PrintResponseDTO> Handle(AttendLateLeaveEarlyPrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<AttendLateLeaveEarlyResponseDTO_Branches>)_mediator.Send(request.Report).Result.Data;
            #region MainData
            var _MainData = new  MainData
            {
                dateFrom = request.Report.DateFrom.ToString(defultData.datetimeFormat),
                dateTo = request.Report.DateTo.ToString(defultData.datetimeFormat)
            };
            var MainData = (_MainData, "MainData");
            #endregion

            #region Lists
            var Branches = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "Branches" };
            var Employees = new GetDatatableDTO { obj = data.SelectMany(c => c.employees).Cast<object>().ToList(), tableName = "Employees" };
            var Days = new GetDatatableDTO { obj = data.SelectMany(c => c.employees).SelectMany(c => c.days).Cast<object>().ToList(), tableName = "Days" };
            if (Days.obj.Count() <= 0)
            {
                var exRes = new ResponseResult
                {
                    Result = Result.Failed
                };
                return new PrintResponseDTO
                {
                    result = exRes
                };
            }
            var lists = new List<GetDatatableDTO>
            {
                Branches,
                Employees,
                Days
            };
            #endregion
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.GetAttendLateLeaveEarlyReport,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType
            };

            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "AttendLateLeaveEarlyReport", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes
            };
        }
    
    }

    public class MainData
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
    }
}
