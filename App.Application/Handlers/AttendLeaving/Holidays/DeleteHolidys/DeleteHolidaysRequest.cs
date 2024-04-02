using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Holidays.DeleteHolidys
{
    public class DeleteHolidaysRequest : App.Domain.Models.Request.AttendLeaving.DeleteHolidays, IRequest<ResponseResult>
    {
    }
}
