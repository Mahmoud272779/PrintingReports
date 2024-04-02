using App.Domain.Models.Request.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.DeleteSectionsAndDepartments
{
    public class DeleteSectionsAndDepartmentsRequest : DeleteSectionsAndDepartmentsDTO, IRequest<ResponseResult>
    {
        public Enums.SectionsAndDepartmentsType Type { get; set; }  
    }
}
