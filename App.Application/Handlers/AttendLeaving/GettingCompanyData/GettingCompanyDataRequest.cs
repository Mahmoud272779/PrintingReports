using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.GettingCompanyData
{
    public class GettingCompanyDataRequest : IRequest<ResponseResult>
    {
        public string companyUniqueName { get; set; }
    }
}
