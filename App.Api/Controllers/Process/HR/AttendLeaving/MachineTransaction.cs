using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.AttendanceMachines.machinesDropdownList;
using App.Application.Handlers.AttendLeaving.EditedMachineTransaction.AddEditedMachineTransaction;
using App.Application.Handlers.AttendLeaving.EditedMachineTransaction.DeleteEditedMachineTransaction;
using App.Application.Handlers.AttendLeaving.EditedMachineTransaction.EditEditedMachineTransaction;
using App.Application.Handlers.AttendLeaving.EditedMachineTransaction.GetEditedMachineTransaction;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class MachineTransaction : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;


        public MachineTransaction(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddEditedMachineTransaction))]
        public async Task<IActionResult> AddEditedMachineTransaction([FromBody] AddEditedMachineTransactionRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.machineTransaction, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpPut(nameof(EditEditedMachineTransaction))]
        public async Task<IActionResult> EditEditedMachineTransaction([FromBody] EditEditedMachineTransactionRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.machineTransaction, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpDelete(nameof(DeleteEditedMachineTransaction))]
        public async Task<IActionResult> DeleteEditedMachineTransaction([FromBody] DeleteEditedMachineTransactionRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.machineTransaction, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetEditedMachineTransaction))]
        public async Task<IActionResult> GetEditedMachineTransaction([FromQuery] GetEditedMachineTransactionRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.machineTransaction, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

    }
}
