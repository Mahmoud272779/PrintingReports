using App.Domain.Models.Request.AttendLeaving;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.AttendanceMachines.EditMachine
{
    public class EditMachineRequest : EditMachineDTO,IRequest<ResponseResult>
    {
    }
}
