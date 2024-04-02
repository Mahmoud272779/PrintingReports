using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Shifts.EditNormalShiftDays;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.ChangefulTime_AddEmps;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.ChangefulTime_GetEmps;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.EditChangefulTimeDays;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.EditChangefulTimeGroupsMaster;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.GetChangefulTimeDays;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.GetChangefulTimeGroupsMaster;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTimeGroupsMaster;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.GetShiftDetailsForEmpIdDay;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.AddMasterShift;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.DeleteMasterShift;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.EditMasterShift;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.GetMasterShift;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Request.AttendLeaving;
using App.Domain.Models.Shared;
using DocumentFormat.OpenXml.Office2010.Drawing;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class ShiftsController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public ShiftsController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }


        //Master Shift
        [HttpPost(nameof(AddMasterShift))]
        public async Task<IActionResult> AddMasterShift([FromBody] AddMasterShiftRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpPut(nameof(EditMasterShift))]
        public async Task<IActionResult> EditMasterShift([FromBody] EditMasterShiftRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        
        
        [HttpDelete(nameof(DeleteMasterShift))]
        public async Task<IActionResult> DeleteMasterShift([FromBody] DeleteMasterShiftRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        
        [HttpGet(nameof(GetMasterShift))]
        public async Task<IActionResult> GetMasterShift([FromQuery] GetMasterShiftRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(GetShiftMasterDropDownlist))]
        public async Task<IActionResult> GetShiftMasterDropDownlist([FromQuery] GetShiftMasterDropDownlistRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetNormalShiftDays))]
        public async Task<IActionResult> GetNormalShiftDays([FromQuery] App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.GetNormalShiftDays.GetNormalShiftDaysRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
        //Master Shift


        //Master Shift




        //Normal Shift
        [HttpPut(nameof(EditNormalShiftDays))]
        public async Task<IActionResult> EditNormalShiftDays([FromForm] EditNormalShiftDaysRequest requestForm)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = new ResponseResult { Result = Enums.Result.Failed};
                 res = await CommandAsync<ResponseResult>(requestForm);
            //if(requestForm.shiftId != 0 )
            //else if(request.shiftId != 0)
                //res = await CommandAsync<ResponseResult>(request);

            return Ok(res);
        } 

        //Normal Shift





        //ChangefulTime
        [HttpPost(nameof(AddChangefulTimeGroupsMaster))]
        public async Task<IActionResult> AddChangefulTimeGroupsMaster([FromBody] AddChangefulTimeGroupsMasterRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        
        [HttpPut(nameof(EditChangefulTimeDays))]
        public async Task<IActionResult> EditChangefulTimeDays([FromBody] EditChangefulTimeDaysRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        } 


        [HttpPut(nameof(EditChangefulTimeGroupsMaster))]
        public async Task<IActionResult> EditChangefulTimeGroupsMaster([FromBody] EditChangefulTimeGroupsMasterRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(GetChangefulTimeGroupsMaster))]
        public async Task<IActionResult> GetChangefulTimeGroupsMaster([FromQuery] GetChangefulTimeGroupsMasterRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
        
        [HttpGet(nameof(GetChangefulTimeDays))]
        public async Task<IActionResult> GetChangefulTimeDays([FromQuery] GetChangefulTimeDaysRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpPut(nameof(ChangefulTime_EditEmps))]
        public async Task<IActionResult> ChangefulTime_EditEmps([FromBody] ChangefulTime_EditEmpsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(ChangefulTime_GetEmps))]
        public async Task<IActionResult> ChangefulTime_GetEmps([FromQuery] ChangefulTime_GetEmpsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetShiftDetailsForEmpIdDay))]
        public async Task<IActionResult> GetShiftDetailsForEmpIdDay([FromQuery] GetShiftDetailsForEmpIdDayRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Shifts, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        //ChangefulTime

    }
}
