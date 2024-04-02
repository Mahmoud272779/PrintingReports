using App.Domain.Models.Request.AttendLeaving.Reports;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Reports.vecationsReport
{
    public  class vecationsReportRequest : vecationsReportRequestDTO,IRequest<ResponseResult>
    {
    }
}
