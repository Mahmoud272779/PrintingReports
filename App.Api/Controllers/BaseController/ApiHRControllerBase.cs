using Attendleave.Erp.Core.APIUtilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.BaseController
{
    [Route("api/HR/[controller]")]
    [ApiController]
    public class ApiHRControllerBase : ApiControllerBase
    {
        public ApiHRControllerBase(IActionResultResponseHandler responseHandler) : base(responseHandler)
        {

        }

        public ApiHRControllerBase(IMediator mediator) : base(mediator)
        {
        }
    }
}
