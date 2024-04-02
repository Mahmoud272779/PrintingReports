using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Missions.AddMissions;
using App.Application.Handlers.AttendLeaving.Missions.DeleteMissions;
using App.Application.Handlers.AttendLeaving.Missions.GetMissions;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using FluentValidation.Internal;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class MissionsController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;
        public MissionsController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }


        [HttpPost(nameof(AddMissions))]
        public async Task<IActionResult> AddMissions([FromBody] AddMissionsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Missions, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpPut(nameof(EditMissions))]
        public async Task<IActionResult> EditMissions([FromBody] EditMissionsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Missions, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpDelete(nameof(DeleteMissions))]
        public async Task<IActionResult> DeleteMissions([FromBody] DeleteMissionsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Missions, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        
        [HttpGet(nameof(GetMissions))]
        public async Task<IActionResult> GetMissions([FromQuery] GetMissionsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Missions, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}
