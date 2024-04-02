using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.VaccationEmployees.AddVaccationEmployees;
using App.Application.Handlers.AttendLeaving.VaccationEmployees.DeleteVaccationEmployees;
using App.Application.Handlers.AttendLeaving.VaccationEmployees.EditVaccationEmployees;
using App.Application.Handlers.AttendLeaving.VaccationEmployees.GetVaccationEmployees;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class VaccationEmployeeController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;
        public VaccationEmployeeController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddVaccationEmployee))]
        public async Task<IActionResult> AddVaccationEmployee([FromBody] AddVaccationEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.VaccationEmployees, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }


        [HttpPut(nameof(EditVaccationEmployee))]
        public async Task<IActionResult> EditVaccationEmployee([FromBody] EditVaccationEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.VaccationEmployees, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }
        [HttpDelete(nameof(DeleteVaccationEmployee))]
        public async Task<IActionResult> DeleteVaccationEmployee([FromBody] DeleteVaccationEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.VaccationEmployees, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);

        }

        [HttpGet(nameof(GetVaccationEmployee))]
        public async Task<IActionResult> GetVaccationEmployee([FromQuery] GetVaccationEmployeesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.VaccationEmployees, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);

        }
    }
}
