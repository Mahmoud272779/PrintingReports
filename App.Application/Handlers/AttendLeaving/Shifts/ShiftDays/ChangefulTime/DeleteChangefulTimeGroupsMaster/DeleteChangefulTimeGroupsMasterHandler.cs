using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.DeleteChangefulTimeGroupsMaster
{
    public class DeleteChangefulTimeGroupsMasterHandler : IRequestHandler<DeleteChangefulTimeGroupsMasterRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsDetalies> _ChangefulTimeGroupsDetaliesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeDays> _ChangefulTimeDaysQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterCommand;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsDetalies> _ChangefulTimeGroupsDetaliesCommand;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeDays> _ChangefulTimeDaysCommand;
        public DeleteChangefulTimeGroupsMasterHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> changefulTimeGroupsMasterQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> changefulTimeGroupsMasterCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsDetalies> changefulTimeGroupsDetaliesQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.ChangefulTimeDays> changefulTimeDaysQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsDetalies> changefulTimeGroupsDetaliesCommand, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.ChangefulTimeDays> changefulTimeDaysCommand)
        {
            _ChangefulTimeGroupsMasterQuery = changefulTimeGroupsMasterQuery;
            _ChangefulTimeGroupsMasterCommand = changefulTimeGroupsMasterCommand;
            _ChangefulTimeGroupsDetaliesQuery = changefulTimeGroupsDetaliesQuery;
            _ChangefulTimeDaysQuery = changefulTimeDaysQuery;
            _ChangefulTimeGroupsDetaliesCommand = changefulTimeGroupsDetaliesCommand;
            _ChangefulTimeDaysCommand = changefulTimeDaysCommand;
        }

        public async Task<ResponseResult> Handle(DeleteChangefulTimeGroupsMasterRequest request, CancellationToken cancellationToken)
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
            var elements = _ChangefulTimeGroupsMasterQuery.TableNoTracking.Include(c => c.changefulTimeGroupsEmployees)
                .Where(c => ids.Contains(c.Id) && !c.changefulTimeGroupsEmployees.Any());
            if (!elements.Any())
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
            _ChangefulTimeGroupsMasterCommand.StartTransaction();


            //delete detalies 
            var ShiftDetalies = _ChangefulTimeGroupsDetaliesQuery.TableNoTracking
                .Where(c => elements.Select(x => x.Id).ToArray().Contains(c.changefulTimeGroupsId));
            _ChangefulTimeGroupsDetaliesCommand.RemoveRange(ShiftDetalies);
            _ChangefulTimeGroupsDetaliesCommand.SaveChanges();

            //delete days
            var days = _ChangefulTimeDaysQuery.TableNoTracking
                .Where(c => elements.Select(x => x.Id).ToArray().Contains(c.changefulTimeGroupsId));
            _ChangefulTimeDaysCommand.RemoveRange(days);
            _ChangefulTimeDaysCommand.SaveChanges();

            //remove group
            _ChangefulTimeGroupsMasterCommand.RemoveRange(elements);
            var saved = await _ChangefulTimeGroupsMasterCommand.SaveChanges() > 0 ? true : false;


            _ChangefulTimeGroupsMasterCommand.CommitTransaction();


            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
