using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Reports.AttendancePermissionsReport;
using App.Application.Handlers.AttendLeaving.Reports.AttendLateLeaveEarly;
using App.Application.Handlers.AttendLeaving.Reports.DayStatus;
using App.Application.Handlers.AttendLeaving.Reports.DetailedAttendance;
using App.Application.Handlers.AttendLeaving.Reports.DetailedDailyLate;
using App.Application.Handlers.AttendLeaving.Reports.GetAbsanceReport;
using App.Application.Handlers.AttendLeaving.Reports.GetReport;
using App.Application.Handlers.AttendLeaving.Reports.GetTotalAbsanceReport;
using App.Application.Handlers.AttendLeaving.Reports.GetTotalLate;
using App.Application.Handlers.AttendLeaving.Reports.TotalAttendance;
using App.Application.Handlers.AttendLeaving.Reports.vecationsReport;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class Reports : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public Reports(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpGet(nameof(DetailedAttendance))]
        public async Task<IActionResult> DetailedAttendance([FromQuery] DetailedAttendanceRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.DetailedAttendance, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
        
        [HttpGet(nameof(DayStatusReport))]
        public async Task<IActionResult> DayStatusReport([FromQuery] DayStatusRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.DayStatusReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetEmployeeReport))]
        public async Task<IActionResult> GetEmployeeReport([FromQuery] GetReportRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.employeeReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetAbsanceReport))]
        public async Task<IActionResult> GetAbsanceReport([FromQuery] GetAbsanceReportRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.AbsanceReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetTotalAbsanceReport))]
        public async Task<IActionResult> GetTotalAbsanceReport([FromQuery] GetTotalAbsanceReportRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.TotalAbsanceReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(TotalAttendance))]
        public async Task<IActionResult> TotalAttendance([FromQuery] TotalAttendanceRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.TotalAttendance, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(vecationsReport))]
        public async Task<IActionResult> vecationsReport([FromQuery] vecationsReportRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.vecationsReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetTotalLateReport))]
        public async Task<IActionResult> GetTotalLateReport([FromQuery] GetTotalLateRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.GetTotalLateReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetDetailedLateReport))]
        public async Task<IActionResult> GetDetailedLateReport([FromQuery] DetailedDailyLateRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.GetDetailedLateReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(GetAttendLateLeaveEarlyReport))]
        public async Task<IActionResult> GetAttendLateLeaveEarlyReport([FromQuery] AttendLateLeaveEarlyRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.GetDetailedLateReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }

        [HttpGet(nameof(AtteandancePermissionsReport))]
        public async Task<IActionResult> AtteandancePermissionsReport([FromQuery] AttendancePermissionsReportRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, subFormCode: (int)SubFormsIds.GetDetailedLateReport, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await QueryAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}
