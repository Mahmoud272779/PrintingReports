using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.AttendancePermission.AddAttendancePermission;
using App.Application.Handlers.AttendLeaving.AttendancePermission.DeleteAttendancePermission;
using App.Application.Handlers.AttendLeaving.AttendancePermission.GetAttendancePermission;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Request.AttendLeaving;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class AttendancePermissionController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public AttendancePermissionController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddAttendancePermission))]
        public async Task<IActionResult> AddAttendancePermission([FromForm] AddAttendancePermissionRequset request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandancePermissions, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync(request);
            return Ok(res);
        }


        [HttpPut(nameof(EditAttendancePermission))]
        public async Task<IActionResult> EditAttendancePermission([FromBody] EditAttendancePermission request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandancePermissions, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync(new AddAttendancePermissionRequset
            {
                isUpdate = true,
                Id = request.Id,
                EmpId = request.EmpId,
                Day = request.Day,
                type = request.type,
                totalHoursForOpenShift = request.totalHoursForOpenShift,
                shift1_start = request.shift1_start,
                shift1_end = request.shift1_end,
                shift2_start = request.shift2_start,
                shift2_end = request.shift2_end,
                shift3_start = request.shift3_start,
                shift3_end = request.shift3_end,
                shift4_start = request.shift4_start,
                shift4_end = request.shift4_end,
                Note = request.Note,
                Has_shift2 = request.Has_shift2,
                Has_shift3 = request.Has_shift3,
                Has_shift4 = request.Has_shift4,
                shift1_IsExtended = request.shift1_IsExtended,
                shift2_IsExtended = request.shift2_IsExtended,
                shift3_IsExtended = request.shift3_IsExtended,
                shift4_IsExtended = request.shift4_IsExtended,

            });
            return Ok(res);

        }


        [HttpDelete(nameof(DeleteAttendancePermission))]
        public async Task<IActionResult> DeleteAttendancePermission([FromBody] DeleteAttendancePermissionRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandancePermissions, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await CommandAsync(request);
            return Ok(res);
        }


        [HttpGet(nameof(GeteAttendancePermission))]
        public async Task<IActionResult> GeteAttendancePermission([FromQuery] GetAttendancePermissionRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.attandancePermissions, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);

            var res = await QueryAsync(request);
            return Ok(res);

        }
    }
}
