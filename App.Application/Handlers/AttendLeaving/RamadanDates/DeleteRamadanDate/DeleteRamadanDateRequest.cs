using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.RamadanDates.DeleteRamadanDate
{
    public class DeleteRamadanDateRequest : App.Domain.Models.Request.AttendLeaving.DeleteRamadanDate, IRequest<ResponseResult>
    {
    }
}
