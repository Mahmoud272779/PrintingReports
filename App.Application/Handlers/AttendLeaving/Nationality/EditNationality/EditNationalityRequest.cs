using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Nationality.EditNationality
{
    public class EditNationalityRequest : App.Domain.Models.Request.AttendLeaving.EditNationality, IRequest<ResponseResult>
    {
    }
}
