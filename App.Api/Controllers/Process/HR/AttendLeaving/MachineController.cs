using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.AddMachine;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.AdvancedSearch_machines;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.DeleteMachine;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.EditMachine;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.GetMachines;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.machinesDropdownList;
using App.Application.Handlers.AttendLeaving.TransactionCancellation;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class MachineController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public MachineController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }
        [HttpPost(nameof(AddMachine))]
        public async Task<IActionResult> AddMachine([FromBody] AddMachineRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandanceMachines, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpPut(nameof(EditMachine))]
        public async Task<IActionResult> EditMachine([FromBody] EditMachineRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandanceMachines, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpDelete(nameof(DeleteMachine))]
        public async Task<IActionResult> DeleteMachine([FromBody] DeleteMachineRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandanceMachines, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(GetMachine))]
        public async Task<IActionResult> GetMachine([FromQuery] GetMachinesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandanceMachines, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpPost(nameof(TransactionCancellation))]
        public async Task<IActionResult> TransactionCancellation([FromBody] TransactionCancellationRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.TransationCancellation, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(machinesDropdownList))]
        public async Task<IActionResult> machinesDropdownList([FromQuery] machinesDropdownListRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandanceMachines, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }


        [HttpGet(nameof(AdvancedSearch_machines))]
        public async Task<IActionResult> AdvancedSearch_machines([FromQuery] AdvancedSearch_machinesRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandanceMachines, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}
