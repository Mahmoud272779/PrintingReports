using App.Domain.Models.Request.AttendLeaving;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EditedMachineTransaction.AddEditedMachineTransaction
{
    public class AddEditedMachineTransactionRequest : AddEditedMachineTransactionDTO, IRequest<ResponseResult>
    {
    }
}
