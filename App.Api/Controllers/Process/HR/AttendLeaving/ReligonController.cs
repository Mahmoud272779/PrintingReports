using System.Threading.Tasks;
using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Holidays.GetHolidays;
using App.Application.Handlers.AttendLeaving.Nationality.AddNationality;
using App.Application.Handlers.AttendLeaving.Nationality.GetNationality;
using App.Application.Handlers.AttendLeaving.Religion.AddReligion;
using App.Application.Handlers.AttendLeaving.Religion.GetAll;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class ReligonController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public ReligonController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddReligion))]
        public async Task<IActionResult> AddReligion([FromBody] AddReligionRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.Religion, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }


        [HttpGet(nameof(GetReligions))]
        public async Task<IActionResult> GetReligions([FromQuery] GetAllRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.Religion, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}

