using App.Application.Handlers.AttendLeaving.Missions.DeleteMissions;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Vaccation.DeleteVaccation
{
    public class DeleteVaccationHandler : IRequestHandler<DeleteVaccationRequest, ResponseResult>

    {

        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Vaccation> _VacctionQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Vaccation> _VacctionCommand;

        public DeleteVaccationHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Vaccation> missionsQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Vaccation> missionsCommand)
        {
            _VacctionQuery = missionsQuery;
            _VacctionCommand = missionsCommand;
        }

        public async Task<ResponseResult> Handle(DeleteVaccationRequest request, CancellationToken cancellationToken)
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
            var elemetsForDelete = _VacctionQuery
                .TableNoTracking
                .Include(c=> c.vaccationEmployees)
                .Where(c => ids.Contains(c.Id) )
                .Where(c=> !c.vaccationEmployees.Any());
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
            _VacctionCommand.RemoveRange(elemetsForDelete);
            var saved = await _VacctionCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
