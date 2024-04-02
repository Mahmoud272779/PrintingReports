using App.Domain.Models.Request.AttendLeaving;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.AddSectionsAndDepartments
{
    public class AddSectionsAndDepartmentsRequest : AddSectionsAndDepartmentsDTO, IRequest<ResponseResult>
    {
        public SectionsAndDepartmentsType Type { get; set; } // 0 for Departments,1 for Sections
    }
}
