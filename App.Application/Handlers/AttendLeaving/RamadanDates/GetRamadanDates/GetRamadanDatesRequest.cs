using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.RamadanDates.GetRamadanDates
{
    public class GetRamadanDatesRequest : App.Domain.Models.Request.AttendLeaving.GetAllRamadanDates, IRequest<ResponseResult>
    {
    }
}
