using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.GetReport;
using App.Application.Handlers.AttendLeaving.Reports.Printing.AttendLateLeaveEarly;
using App.Application.Services.Printing.GenralPrint;
using App.Application.Services.Printing.PrintFile;
using App.Infrastructure.settings;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.Printing.EmployeesReportPrint
{
    public class EmployeesReportPrintHandler : IRequestHandler<EmployeesReportPrintRequest, PrintResponseDTO>
    {
        private readonly IMediator _mediator;
        private readonly IprintFileService _iPrintFileService;
        private readonly IGeneralPrint _iGeneralPrint;
        public EmployeesReportPrintHandler(IMediator mediator, IprintFileService iPrintFileService, IGeneralPrint iGeneralPrint)
        {
            _mediator = mediator;
            _iPrintFileService = iPrintFileService;
            _iGeneralPrint = iGeneralPrint;
        }

        public async Task<PrintResponseDTO> Handle(EmployeesReportPrintRequest request, CancellationToken cancellationToken)
        {
            var data = (List<EmpDTO>)_mediator.Send(request.Report).Result.Data;
           

            #region Lists
            var Employees = new GetDatatableDTO { obj = data.Cast<object>().ToList(), tableName = "Employees" };
           
            var lists = new List<GetDatatableDTO>
            {
                
                Employees
                
            };
            #endregion
            PrintReportDTO printReport = new PrintReportDTO
            {
                isArabic = request.isArabic,
                fileId = request.fileId ?? 0,
                screenId = (int)SubFormsIds.employeeReport,
                lists = lists,
               
                exportType = request.exportType
            };

            var webReport = await _iGeneralPrint.PrintReport(printReport);

            var reportRes = await _iPrintFileService.PrintFile(webReport, "Employee_Report", request.exportType);
            return new PrintResponseDTO
            {
                result = reportRes
            };
        }
    }
}
