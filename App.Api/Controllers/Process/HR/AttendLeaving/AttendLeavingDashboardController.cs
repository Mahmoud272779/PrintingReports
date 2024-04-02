using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Dashboard.AttendingLeaveDetalies;
using App.Application.Handlers.AttendLeaving.Dashboard.LastSevenDaysAttandance;
using App.Application.Handlers.AttendLeaving.Dashboard.LiveStreemPrintFingers;
using App.Application.Handlers.AttendLeaving.Dashboard.SummaryOfAttandanceOfBranches;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class AttendLeavingDashboardController : ApiHRControllerBase
    {
        public AttendLeavingDashboardController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet(nameof(AttendingLeaveDetalies))]
        public async Task<IActionResult> AttendingLeaveDetalies()
        {
            var res = await QueryAsync(new AttendingLeaveDetaliesRequest());
            return Ok(res);
        }
        [HttpGet(nameof(SummaryOfAttandanceOfBranches))]
        public async Task<IActionResult> SummaryOfAttandanceOfBranches()
        {
            var res = await QueryAsync(new SummaryOfAttandanceOfBranchesRequest());
            return Ok(res);
        }
        [HttpGet(nameof(LastSevenDaysAttandance))]
        public async Task<IActionResult> LastSevenDaysAttandance()
        {
            var res = await QueryAsync(new LastSevenDaysAttandanceRequest());
            return Ok(res);
        }
        [HttpGet(nameof(LiveStreemPrintFingers))]
        public async Task<IActionResult> LiveStreemPrintFingers()
        {
            var res = await QueryAsync(new LiveStreemPrintFingersRequest());
            return Ok(res);
        }
    }
}
