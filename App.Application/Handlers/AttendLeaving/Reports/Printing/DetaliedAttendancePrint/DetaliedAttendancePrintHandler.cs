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

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.DetaliedAttendancePrint
{
    public class DetaliedAttendancePrintHandler : IRequestHandler<DetaliedAttendancePrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public DetaliedAttendancePrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }
        public async Task<PrintResponseDTO> Handle(DetaliedAttendancePrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<DetailedAttendanceResponseDTO_Branches>)_mediator.Send(request.Report).Result.Data;
            #region MainData
            var _MainData = new MainData
            {
                dateFrom = request.Report.dateFrom.ToString(defultData.datetimeFormat),
                dateTo = request.Report.dateTo.ToString(defultData.datetimeFormat)
            };
            var MainData = (_MainData, "MainData");
            #endregion

            #region Lists
            var _Branches = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "Branches" };
            var _Employees = new GetDatatableDTO { obj = data.SelectMany(c=> c.employees).Cast<object>().ToList(), tableName = "Employees" };
            var _Days = new GetDatatableDTO { obj = data.SelectMany(c=> c.employees).SelectMany(c=> c.days).Cast<object>().ToList(), tableName = "Days" };
            var lists = new List<GetDatatableDTO>
            {
                _Branches,
                _Employees,
                _Days
            };
            #endregion
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.DetailedAttendance,
                lists = lists,
                MainData = MainData,
                exportType = request.exportType
            };

            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "DetailedAttendance", request.exportType);
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
