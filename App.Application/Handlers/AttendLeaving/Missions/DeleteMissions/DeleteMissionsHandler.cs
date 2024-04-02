using App.Domain.Entities.Process.AttendLeaving;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Missions.DeleteMissions
{
    public class DeleteMissionsHandler : IRequestHandler<DeleteMissionsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Missions> _MissionsQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Missions> _MissionsCommand;

        public DeleteMissionsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Missions> missionsQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Missions> missionsCommand)
        {
            _MissionsQuery = missionsQuery;
            _MissionsCommand = missionsCommand;
        }

        public async Task<ResponseResult> Handle(DeleteMissionsRequest request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Split(',').Select(c=> int.Parse(c)).ToArray();
            if(!ids.Any())
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
            var elemetsForDelete = _MissionsQuery.TableNoTracking.Include(c => c.employees).Where(c => ids.Contains(c.Id) && !c.employees.Any());
            if(!elemetsForDelete.Any())
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
            _MissionsCommand.RemoveRange(elemetsForDelete);
            var saved = await _MissionsCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
