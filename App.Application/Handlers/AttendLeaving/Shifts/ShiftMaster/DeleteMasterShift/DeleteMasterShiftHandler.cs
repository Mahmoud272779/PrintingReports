using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftMaster.DeleteMasterShift
{
    public class DeleteMasterShiftHandler : IRequestHandler<DeleteMasterShiftRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<ShiftsMaster> _ShiftsMasterCommand;
        private readonly IRepositoryQuery<ShiftsMaster> _ShiftsMasterQuery;

        public DeleteMasterShiftHandler(IRepositoryCommand<ShiftsMaster> shiftsMasterCommand, IRepositoryQuery<ShiftsMaster> shiftsMasterQuery)
        {
            _ShiftsMasterCommand = shiftsMasterCommand;
            _ShiftsMasterQuery = shiftsMasterQuery;
        }

        public async Task<ResponseResult> Handle(DeleteMasterShiftRequest request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Split(',').Select(c => int.Parse(c)).ToArray();
            if (!ids.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.DataRequired,
                        MessageEn = ErrorMessagesEn.DataRequired,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            var elemetsForDelete = _ShiftsMasterQuery.TableNoTracking.Include(c => c.InvEmployees)
                .Include(c=>c.normalShiftDetalies)
                .Include(c=>c.changefulTimeGroups)
                .Where(c => ids.Contains(c.Id) && !c.InvEmployees.Any() && !c.normalShiftDetalies.Any() && !c.changefulTimeGroups.Any());
            if (!elemetsForDelete.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.CanNotDelete,
                        MessageEn = ErrorMessagesEn.CanNotDelete,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            _ShiftsMasterCommand.RemoveRange(elemetsForDelete);
            var saved = await _ShiftsMasterCommand.SaveChanges() > 0 ? true : false;

            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
