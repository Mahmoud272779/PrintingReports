using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Holidays.AddHolidays;
using App.Application.Handlers.AttendLeaving.Holidays.DeleteHolidys;
using App.Application.Handlers.AttendLeaving.Holidays.EditHolidays;
using App.Application.Handlers.AttendLeaving.Holidays.EditHolidaysEmployees;
using App.Application.Handlers.AttendLeaving.Holidays.GetHolidays;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class HolidaysController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;
        public HolidaysController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }
        [HttpPost(nameof(AddHolidays))]
        public async Task<IActionResult> AddHolidays([FromBody] AddHolidaysRequest request) 
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }

        [HttpPut(nameof(EditHolidays))]
        public async Task<IActionResult> EditHolidays([FromBody] EditHolidaysRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }
        [HttpDelete(nameof(DeleteHolidays))]
        public async Task<IActionResult> DeleteHolidays([FromBody] DeleteHolidaysRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }

        [HttpGet(nameof(GeteHolidays))]
        public async Task<IActionResult> GeteHolidays( [FromQuery] GetHolidaysRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);

        }


        [HttpPut(nameof(EditHolidaysEmployees))]
        public async Task<IActionResult> EditHolidaysEmployees([FromQuery] EditHolidaysEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Holidays, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }


    }
}
