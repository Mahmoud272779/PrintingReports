using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.HolidaysEmployees.AddHolidaysEmployees;
using App.Application.Handlers.AttendLeaving.HolidaysEmployees.DeleteHolidaysEmployees;
using App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetEmployeesForAddModel;
using App.Application.Handlers.AttendLeaving.HolidaysEmployees.GetHolidaysEmployees;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class HolidaysEmployeesController  : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public HolidaysEmployeesController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddHolidaysEmployees))]
        public async Task<IActionResult> AddHolidaysEmployees([FromBody] AddHolidaysEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }

        [HttpDelete(nameof(DeleteHolidaysEmployees))]
        public async Task<IActionResult> DeleteHolidaysEmployees([FromBody] DeleteHolidaysEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }

        [HttpGet(nameof(GeteHolidaysEmployees))]
        public async Task<IActionResult> GeteHolidaysEmployees([FromQuery] GetHolidaysEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);

        }
        [HttpGet(nameof(GetEmployeesForAddModel))]
        public async Task<IActionResult> GetEmployeesForAddModel([FromQuery] GetEmployeesForAddModelRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);

        }
    }
}
