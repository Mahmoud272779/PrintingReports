using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeeInGroup;
using App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeesInGroup;
using App.Application.Handlers.AttendLeaving.EmployeeGroups.DeleteEmployeesFromGroup;
using App.Application.Handlers.AttendLeaving.EmployeeGroups.GetEmployeesInCertainGroup;
using App.Application.Handlers.AttendLeaving.Missions.AddMissions;
using App.Application.Handlers.AttendLeaving.Missions.DeleteMissions;
using App.Application.Handlers.AttendLeaving.Missions.GetMissions;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Request.AttendLeaving;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class EmployeeGroupsController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;
        public EmployeeGroupsController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }


        [HttpPost(nameof(AddEmployeesGroup))]
        public async Task<IActionResult> AddEmployeesGroup([FromBody] AddEmployeesGroupRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }



        [HttpPost(nameof(AddEmployeesInCertainGroup))]
        public async Task<IActionResult> AddEmployeesInCertainGroup([FromBody] AddEmployeesInGroupRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }


        [HttpPut(nameof(EditEmployeeGroups))]
        public async Task<IActionResult> EditEmployeeGroups([FromBody] EditEmployeeGroupsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpDelete(nameof(DeleteEmployeesGroup))]
        public async Task<IActionResult> DeleteEmployeesGroup([FromBody] DeleteEmployeeGroupsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }


        [HttpDelete(nameof(DeleteEmployeesFromGroup))]
        public async Task<IActionResult> DeleteEmployeesFromGroup([FromBody] DeleteEmployeesFromGroupRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetEmployeeGroups))]
        public async Task<IActionResult> GetEmployeeGroups([FromQuery] GetEmployeeGroupsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetEmployeesNotInGroups))]
        public async Task<IActionResult> GetEmployeesNotInGroups([FromQuery] GetEmployeesForAddModelRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }


        [HttpGet(nameof(GetEmployeesInCertainGroups))]
        public async Task<IActionResult> GetEmployeesInCertainGroups([FromQuery] GetEmployeesInCertainGroupRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.EmployeeGroups, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}
