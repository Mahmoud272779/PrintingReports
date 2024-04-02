using App.Api.Controllers.BaseController;
using App.Application.Handlers.AttendLeaving.Missions.GetMissions;
using App.Application.Handlers.AttendLeaving.Projects.AddProject;
using App.Application.Handlers.AttendLeaving.Projects.DeleteProjects;
using App.Application.Handlers.AttendLeaving.Projects.EditProject;
using App.Application.Services.HelperService.authorizationServices;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Models.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace App.Api.Controllers.Process.HR.AttendLeaving
{
    public class projectsController : ApiHRControllerBase
    {
        private readonly iAuthorizationService _iAuthorizationService;

        public projectsController(IMediator mediator, iAuthorizationService iAuthorizationService) : base(mediator)
        {
            _iAuthorizationService = iAuthorizationService;
        }
        [HttpPost(nameof(AddProject))]
        public async Task<IActionResult> AddProject([FromBody] AddProjectRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.projects, Opretion.Add);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpPut(nameof(EditProject))]
        public async Task<IActionResult> EditProject([FromBody] EditProjectRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.projects, Opretion.Edit);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpDelete(nameof(DeleteProject))]
        public async Task<IActionResult> DeleteProject([FromBody] DeleteProjectsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.projects, Opretion.Delete);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
        [HttpGet(nameof(GetProject))]
        public async Task<IActionResult> GetProject([FromQuery] GetProjectsRequest request)
        {
            var isAuthorized = await _iAuthorizationService.isAuthorized(0, (int)SubFormsIds.projects, Opretion.Open);
            if (isAuthorized != null)
                return Ok(isAuthorized);
            var res = await CommandAsync<ResponseResult>(request);
            return Ok(res);
        }
    }
}
