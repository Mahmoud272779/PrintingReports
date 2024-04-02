using System.Threading.Tasks;
using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Missions.GetMissions;
using App.Application.Handlers.AttendLeaving.Projects.AddProject;
using App.Application.Handlers.AttendLeaving.Projects.DeleteProjects;
using App.Application.Handlers.AttendLeaving.Projects.EditProject;
using App.Application.Handlers.AttendLeaving.RamadanDates.AddRamadanDate;
using App.Application.Handlers.AttendLeaving.RamadanDates.DeleteRamadanDate;
using App.Application.Handlers.AttendLeaving.RamadanDates.EditRamadanDate;
using App.Application.Handlers.AttendLeaving.RamadanDates.GetRamadanDates;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Request.AttendLeaving;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    
    public class RamadanDatesController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public RamadanDatesController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }
        [HttpPost(nameof(AddRamadanDate))]
        public async Task<IActionResult> AddRamadanDate([FromBody] AddRamadanDateRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.RamadanDates, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpPut(nameof(EditRamadanDate))]
        public async Task<IActionResult> EditRamadanDate([FromBody] EditRamadanDateRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.RamadanDates, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpDelete(nameof(DeleteRamadanDate))]
        public async Task<IActionResult> DeleteRamadanDate([FromBody] DeleteRamadanDateRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.RamadanDates, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(GetRamadanDates))]
        public async Task<IActionResult> GetRamadanDates([FromQuery] GetRamadanDatesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.RamadanDates, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}
