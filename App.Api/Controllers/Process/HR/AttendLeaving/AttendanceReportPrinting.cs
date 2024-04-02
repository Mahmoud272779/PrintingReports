using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Reports.Printing.AbsancePrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.Printing.DayStatusPrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.DetailedDailyLatePrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.DetaliedAttendancePrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.EmployeesReportPrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.TotalLatePrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.GetTotalAbsancePrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.TotalVecationsPrint;
using App.Application.Handlers.AttendLeaving.Reports.Printing.vecationsPrint;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Reports.Printing.AttendancePermissionPrint;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class AttendanceReportPrinting : ApiHRControllerBase
    {
        public AttendanceReportPrinting(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet(nameof(DayStatusPrint))]
        public async Task<IActionResult> DayStatusPrint([FromQuery] DayStatusPrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }

        [HttpGet(nameof(TotalLatePrint))]
        public async Task<IActionResult> TotalLatePrint([FromQuery] TotalLatePrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }


        [HttpGet(nameof(DetaliedAttendancePrint))]
        public async Task<IActionResult> DetaliedAttendancePrint([FromQuery] DetaliedAttendancePrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }
        [HttpGet(nameof(AttendLateLeaveEarlyPrint))]
        public async Task<IActionResult> AttendLateLeaveEarlyPrint([FromQuery] AttendLateLeaveEarlyPrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }
        [HttpGet(nameof(TotalVecationsReportPrint))]
        public async Task<IActionResult> TotalVecationsReportPrint([FromQuery] vecationsPrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }

        [HttpGet(nameof(TotalAttendancePrint))]
        public async Task<IActionResult> TotalAttendancePrint([FromQuery] TotalAttendancePrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }
        [HttpGet(nameof(TotalAbsancePrint))]
        public async Task<IActionResult> TotalAbsancePrint([FromQuery] GetTotalAbsancePrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }
        [HttpGet(nameof(AbsancePrint))]
        public async Task<IActionResult> AbsancePrint([FromQuery] AbsancePrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }

        [HttpGet(nameof(DetailedDailyLatePrint))]
        public async Task<IActionResult> DetailedDailyLatePrint([FromQuery] DetailedDailyLatePrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }

        [HttpGet(nameof(EmployeeReportPrint))]
        public async Task<IActionResult> EmployeeReportPrint([FromQuery] EmployeesReportPrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }


        [HttpGet(nameof(AttendancePermissionsPrint))]
        public async Task<IActionResult> AttendancePermissionsPrint([FromQuery] AttendancePermissionPrintRequest request)
        {
            var res = await QueryAsync(request);
            return Ok(res);
        }

    }
}
