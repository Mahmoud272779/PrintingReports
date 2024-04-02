using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.RamadanDates.AddRamadanDate
{
    public class AddRamadanDateRequest : App.Domain.Models.Request.AttendLeaving.AddRamadanDate, IRequest<ResponseResult>
    {
    }
}
