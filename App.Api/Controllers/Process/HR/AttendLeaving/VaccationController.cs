using App.Api.Controllers.BaseController;

using App.Application.Handlers.AttendLeaving.Vaccation.AddVaccation;
using App.Application.Handlers.AttendLeaving.Vaccation.DeleteVaccation;
using App.Application.Handlers.AttendLeaving.Vaccation.EditVaccation;
using App.Application.Handlers.AttendLeaving.Vaccation.GetVaccation;
using App.Application.Handlers.AttendLeaving.Vaccation.GetVaccationDropDownList;
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
    public class VaccationController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;
        public VaccationController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddVaccation))]
        public async Task<IActionResult> AddVaccation([FromBody] AddVaccationRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Vaccation, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpPut(nameof(EditVaccation))]
        public async Task<IActionResult> EditVaccation([FromBody] EditVaccationRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Vaccation, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }
        [HttpDelete(nameof(DeleteVaccation))]
        public async Task<IActionResult> DeleteVaccation([FromBody] DeleteVaccationRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Vaccation, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }

        [HttpGet(nameof(GetVaccation))]
        public async Task<IActionResult> GetVaccation([FromQuery] GetVaccationRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Vaccation, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);

        }
        [HttpGet(nameof(GetVaccationDropDown))]
        public async Task<IActionResult> GetVaccationDropDown([FromQuery] GetVaccationDropDownRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Vaccation, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);

        }


        

    }
}
