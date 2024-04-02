using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Nationality.GetNationality
{
    public class GetNationalityRequest : App.Domain.Models.Request.AttendLeaving.GetNationality, IRequest<ResponseResult>
    {
    }
}
