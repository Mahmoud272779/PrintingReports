using System.Threading.Tasks;
using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.AttendLeaving_Settings.EditAttendLeaving_Settings;
using App.Application.Handlers.AttendLeaving.AttendLeaving_Settings.GetAttendLeaving_Settings;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class AttendLeaving_SettingsController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;
        public AttendLeaving_SettingsController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPut(nameof(EditAttendLeaving_Settings))]
        public async Task<IActionResult> EditAttendLeaving_Settings([FromBody] EditAttendLeaving_SettingsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.AttendLeaving_Settings, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetAttendLeaving_Settings))]
        public async Task<IActionResult> GetAttendLeaving_Settings()
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.AttendLeaving_Settings, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(new GetAttendLeaving_SettingsRequest());
            return Ok(res);
        }


    }
}
