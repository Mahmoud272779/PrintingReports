using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.AddHolidays
{
    public class AddHolidaysRequest : App.Domain.Models.Request.AttendLeaving.AddHolidays, IRequest<ResponseResult>
    {
    }
}
