using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.PushDataAPI;
using App.Infrastructure.settings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class PushDataServicesController : ApiHRControllerBase
    {
        public PushDataServicesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("PushDataAPI")]
        [AllowAnonymous]
        public async Task<IActionResult> PushDataAPI([FromBody] PushDataAPIRequest request)
        {
            if (request.SecKey != defultData.userManagmentApplicationSecurityKey)
                return BadRequest();
            var res = await CommandAsync<bool>(request);
            return Ok(res);
        }
    }
}
