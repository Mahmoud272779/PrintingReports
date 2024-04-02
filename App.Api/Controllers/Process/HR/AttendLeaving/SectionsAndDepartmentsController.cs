using App.Api.Controllers.BaseController;
using App.Application;
using App.Application.Handlers.AttendLeaving.SectionsAndDepartments.AddSectionsAndDepartments;
using App.Application.Handlers.AttendLeaving.SectionsAndDepartments.DeleteSectionsAndDepartments;
using App.Application.Handlers.AttendLeaving.SectionsAndDepartments.EditSectionsAndDepartments;
using App.Application.Handlers.AttendLeaving.SectionsAndDepartments.GetSectionsAndDepartments;
using App.Application.Handlers.AttendLeaving.SectionsAndDepartments.SectionsAndDepartmentsDropdownList;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Request.AttendLeaving;
using App.Domain.Models.Shared;
using Attendleave.Erp.Core.APIUtilities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class SectionsAndDepartmentsController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public SectionsAndDepartmentsController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }

        [HttpPost(nameof(AddSections))]
        public async Task<IActionResult> AddSections([FromBody] AddSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Add);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await CommandAsync<ResponseResult>(new AddSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Sections,
                arabicName = request.arabicName,
                latinName = request.latinName,
                empId = request.empId,
                parentId = request.parentId
            });
            return Ok(res);
        }
        [HttpPost(nameof(AddDepartments))]
        public async Task<IActionResult> AddDepartments([FromBody] AddSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Add);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await CommandAsync<ResponseResult>(new AddSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Departments,
                arabicName = request.arabicName,
                latinName = request.latinName,
                empId = request.empId,
                parentId = request.parentId
            });
            return Ok(res);
        }




        [HttpPut(nameof(EditSections))]
        public async Task<IActionResult> EditSections([FromBody] EditSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Edit);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await CommandAsync<ResponseResult>(new EditSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Sections,
                arabicName = request.arabicName,
                latinName = request.latinName,
                empId = request.empId,
                parentId = request.parentId,
                Id = request.Id
            });
            return Ok(res);
        }

        [HttpPut(nameof(EditDepartments))]
        public async Task<IActionResult> EditDepartments([FromBody] EditSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Edit);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await CommandAsync<ResponseResult>(new EditSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Departments,
                arabicName = request.arabicName,
                latinName = request.latinName,
                empId = request.empId,
                parentId = request.parentId,
                Id = request.Id
            });
            return Ok(res);
        }



        [HttpDelete(nameof(DeleteSection))]
        public async Task<IActionResult> DeleteSection([FromBody] DeleteSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Delete);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await CommandAsync<ResponseResult>(new DeleteSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Sections,
                Id = request.Id

            });
            return Ok(res);
        }
        [HttpDelete(nameof(DeleteDepartments))]
        public async Task<IActionResult> DeleteDepartments([FromBody] DeleteSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Delete);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await CommandAsync<ResponseResult>(new DeleteSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Departments,
                Id = request.Id

            });
            return Ok(res);
        }




        [HttpGet(nameof(GetSections))]
        public async Task<IActionResult> GetSections([FromQuery] GetSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Open);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await QueryAsync<ResponseResult>(new GetSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Sections,
                searchCriteria = request.searchCriteria,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                ParentId = request.ParentId

            });
            return Ok(res);
        }
        [HttpGet(nameof(GetDepartments))]
        public async Task<IActionResult> GetDepartments([FromQuery] GetSectionsAndDepartmentsDTO request)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Open);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await QueryAsync<ResponseResult>(new GetSectionsAndDepartmentsRequest
            {
                Type = Enums.SectionsAndDepartmentsType.Departments,
                searchCriteria = request.searchCriteria,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                ParentId = request.ParentId

            });
            return Ok(res);
        }



        [HttpGet(nameof(GetSectionsDropDownList))]
        public async Task<IActionResult> GetSectionsDropDownList([FromQuery] string parentId)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Open);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await QueryAsync<ResponseResult>(new SectionsAndDepartmentsDropdownListRequest
            {
                parentId = parentId,
                Type = Enums.SectionsAndDepartmentsType.Sections
            });
            return Ok(res);
        }
        [HttpGet(nameof(GetDepartmentsDropDownList))]
        public async Task<IActionResult> GetDepartmentsDropDownList([FromQuery] string parentId)
        {
            var isAuthorised = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.Branches_MainData, Opretion.Open);
            if (isAuthorised != null)
                return Ok(isAuthorised);
            var res = await QueryAsync<ResponseResult>(new SectionsAndDepartmentsDropdownListRequest
            {
                parentId = parentId,
                Type = Enums.SectionsAndDepartmentsType.Departments
            });
            return Ok(res);
        }
    }
}
