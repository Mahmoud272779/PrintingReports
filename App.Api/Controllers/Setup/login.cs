using App.Api.Controllers.BaseController;
using App.Application;
using App.Application.Handlers.ForgetPassword;
using App.Application.Handlers.Helper;
using App.Application.Handlers.Login.LockAccount;
using App.Application.Handlers.Login.ResumeSessionService;
using App.Application.Services.HelperService.CookiesAppend;
using App.Domain;
using Attendleave.Erp.Core.APIUtilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace App.Api.Controllers.Setup
{
    public class login : ApiControllerBase
    {
        private readonly iLoginService _iLoginService;
        private readonly ICookiesService _cookiesService;
        private readonly IMediator mediator;

        public login(IActionResultResponseHandler responseHandler,
                     iLoginService iLoginService, IMemoryCache memoryCache,
                     ICookiesService cookiesService, IMediator mediator) : base(responseHandler)
        {
            _iLoginService = iLoginService;
            _cookiesService = cookiesService;
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody]loginReqDTO loginDTO)
        {
            var res = await _iLoginService.login(loginDTO);
            return Ok(res);
        }
        [AllowAnonymous]
        [HttpPost(nameof(ForgetPassword))]
        public async Task<IActionResult> ForgetPassword([FromBody] forgetPasswordRequest parm)
        {
            var res = await mediator.Send(parm);
            return Ok(res);
        }

        [Authorize]
        [HttpPost(nameof(ResumeSession))]
        public async Task<IActionResult> ResumeSession([FromBody] ResumeSessionRequest parm)
        {
            var res = await mediator.Send(parm);
            return Ok(res);
        }
        [Authorize]
        [HttpPost(nameof(lockAccount))]
        public async Task<IActionResult> lockAccount([FromQuery] lockAccountRequest parm)
        {
            var res = await mediator.Send(parm);
            return Ok(res);
        }
        [Authorize]
        [HttpGet(nameof(GetCompanySubscriptionInformation))]
        public async Task<IActionResult> GetCompanySubscriptionInformation()
        {
            var res = await mediator.Send(new Application.Handlers.Settings.companyInformationRequest());
            return Ok(res);
        }
        [Authorize]
        [HttpGet(nameof(currentUserSettings))]
        public async Task<IActionResult> currentUserSettings()
        {
            var res = await mediator.Send(new currentUserSettingsRequest());
            return Ok(res);
        }
    }
}
