using App.Domain.Models.Request.AttendLeaving;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.GetSectionsAndDepartments
{
    public class GetSectionsAndDepartmentsRequest : GetSectionsAndDepartmentsDTO,IRequest<ResponseResult>
    {
        public SectionsAndDepartmentsType Type { get; set; }
    }
}
