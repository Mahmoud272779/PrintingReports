using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.EditHolidays
{
    public class EditHolidaysRequest : App.Domain.Models.Request.AttendLeaving.EditHolidays , IRequest<ResponseResult>
    {
    }
}
