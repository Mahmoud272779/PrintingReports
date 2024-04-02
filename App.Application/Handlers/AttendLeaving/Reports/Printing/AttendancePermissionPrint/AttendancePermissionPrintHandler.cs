using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.AttendancePermissionsReport;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.Printing.AttendLateLeaveEarly;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Infrastructure.settings;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.AttendancePermissionPrint
{
    public class AttendancePermissionPrintHandler : IRequestHandler<AttendancePermissionPrintRequest, ResponseResult>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public AttendancePermissionPrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }


        public async Task<ResponseResult> Handle(AttendancePermissionPrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<AttendancePermisionsReportResponseDTO_Branches>)_mediator.Send(request.Report).Result.Data;
            #region MainData
            var _MainData = new MainData
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
            var Permissions = new GetDatatableDTO { obj = data.SelectMany(c => c.employees).SelectMany(c => c.days).SelectMany(c=>c.permissions).Cast<object>().ToList(), tableName = "Permissions" };
            if (Days.obj.Count() <= 0)
                return new ResponseResult
                {
                    Result = Result.Failed
                };
            var lists = new List<GetDatatableDTO>
            {
                Branches,
                Employees,
                Days,
                Permissions
            };
            #endregion
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.AttendancePermissionsReport,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType
            };

            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "AttendancePermissionsReport", request.exportType);
            return new ResponseResult()
            {
                Data = reportRes,
                Result = Result.Success
            };
        }
    }

    public class MainData
    {
        public string dateFrom { get; set; }
        public string dateTo { get; set; }
    }

}
