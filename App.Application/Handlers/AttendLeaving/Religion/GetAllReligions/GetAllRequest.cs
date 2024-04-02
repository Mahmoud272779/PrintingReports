using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Religion.GetAll
{
    public class GetAllRequest : PaginationVM , IRequest<ResponseResult>
    {
        public string? searchCriteria { get; set; }
    
    }
}
