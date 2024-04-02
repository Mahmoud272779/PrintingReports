using App.Domain.Models.Request.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EditedMachineTransaction.EditEditedMachineTransaction
{
    public class EditEditedMachineTransactionRequest : EditEditedMachineTransactionDTO, IRequest<ResponseResult>
    {
    }
}
