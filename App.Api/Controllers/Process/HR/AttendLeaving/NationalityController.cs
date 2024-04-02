using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Nationality.AddNationality;
using App.Application.Handlers.AttendLeaving.Nationality.DeleteNationality;
using App.Application.Handlers.AttendLeaving.Nationality.EditNationality;
using App.Application.Handlers.AttendLeaving.Nationality.GetNationality;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class NationalityController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public NationalityController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddNationality))]
        public async Task<IActionResult> AddNationality([FromBody] AddNationalityRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.Nationality, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpPut(nameof(EditNationality))]
        public async Task<IActionResult> EditNationality([FromBody] EditNationalityRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.Nationality, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpDelete(nameof(DeleteNationality))]
        public async Task<IActionResult> DeleteNationality([FromBody] DeleteNationalityRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.Nationality, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetNationality))]
        public async Task<IActionResult> GetNationality([FromQuery] GetNationalityRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.Nationality, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}
