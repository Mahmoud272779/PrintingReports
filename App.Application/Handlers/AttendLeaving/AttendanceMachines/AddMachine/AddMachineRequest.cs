using App.Domain.Models.Request.AttendLeaving;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.AddMachine
{
    public class AddMachineRequest : AddMachineDTO,IRequest<ResponseResult>
    {
    }
}
