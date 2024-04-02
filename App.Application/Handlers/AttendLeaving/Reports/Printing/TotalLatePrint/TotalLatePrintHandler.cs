using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.GetTotalLate;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Infrastructure.settings;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.TotalLatePrint
{
    public class TotalLatePrintHandler : IRequestHandler<TotalLatePrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;

        public TotalLatePrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }

       
        public async Task<PrintResponseDTO> Handle(TotalLatePrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<BranchDTOLate>)_mediator.Send(request.Report).Result.Data;

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
            var Employees = new GetDatatableDTO { obj = data.SelectMany(c => c.Employees).Cast<object>().ToList(), tableName = "Employees" };

           
            var lists = new List<GetDatatableDTO>
            {
                Branches,
                Employees,
                
            };
            #endregion
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.GetTotalLateReport,
                MainData = MainData,
                lists = lists,
                
                exportType = request.exportType
            };

            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "TotalLate_Report", request.exportType);
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
