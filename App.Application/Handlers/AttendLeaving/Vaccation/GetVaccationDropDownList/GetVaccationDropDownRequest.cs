using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.GetVaccationDropDownList
{
    public class GetVaccationDropDownRequest : IRequest<ResponseResult>
    {
        public string? SearchCriteria { get; set; }
    }
}
