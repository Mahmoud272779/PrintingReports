using System.Linq;
using System.Threading.Tasks;
using App.Api.Controllers.BaseController;
using App.Application.Handlers.Profit;
using App.Application.Helpers;
using App.Application.Services.HelperService.authorizationServices;
using App.Application.Services.Process.Branches;
using App.Application.Services.Process.Category;
using App.Domain.Entities;
using App.Domain.Entities.Process;
using App.Domain.Enums;
using App.Domain.Models.Common;
using App.Domain.Models.Security.Authentication.Request;
using App.Domain.Models.Shared;
using App.Infrastructure.Interfaces.Repository;
using Attendleave.Erp.Core.APIUtilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static App.Domain.Enums.Enums;

namespace App.Api.Controllers.Process
{
    public class TestProfitController : ApiStoreControllerBase
    {
        private readonly ICategoryService CategoriesService;
        private readonly iAuthorizationService _iAuthorizationService;
        private readonly IPrepareDataForProfit prepareDataForProfit;
        private readonly iUserInformation _iUserInformation;
        private readonly IRepositoryQuery<GLBranch> _branchBusiness;

        public TestProfitController(IPrepareDataForProfit PrepareDataForProfit,
            iUserInformation userInformationModel ,
            IRepositoryQuery <GLBranch> branchBusiness,   
            IActionResultResponseHandler ResponseHandler) : base(ResponseHandler)
        {
            prepareDataForProfit = PrepareDataForProfit;
            _iUserInformation = userInformationModel;
            _branchBusiness = branchBusiness;
        }

        
        [HttpGet("startCalculateProfit")]
        public async Task<ResponseResult> startCalculateProfit()
        {var userInfo = await _iUserInformation.GetUserInformation();
                var data =
                _branchBusiness.TableNoTracking
                .Where(e => userInfo.employeeBranches.Contains(e.Id) && e.Status == (int)Status.Active)
                .Select(a => a.Id ).ToList();
            
            var result = await prepareDataForProfit.PreparingDataForProfit(1);
            
            return result;
        }


    }
}
