using App.Domain.Models.Request.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.EditSectionsAndDepartments
{
    public class EditSectionsAndDepartmentsRequest : EditSectionsAndDepartmentsDTO,IRequest<ResponseResult>
    {
        public SectionsAndDepartmentsType Type { get; set; }
    }
}
