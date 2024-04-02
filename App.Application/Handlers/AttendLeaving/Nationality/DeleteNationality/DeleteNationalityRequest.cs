using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Nationality.DeleteNationality
{
    public class DeleteNationalityRequest : App.Domain.Models.Request.AttendLeaving.DeleteNationality, IRequest<ResponseResult>
    {
    }
}
