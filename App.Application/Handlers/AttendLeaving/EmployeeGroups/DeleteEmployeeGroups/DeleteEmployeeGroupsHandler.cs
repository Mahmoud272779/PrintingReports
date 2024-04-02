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
    public class DeleteEmployeeGroups : IRequestHandler<DeleteEmployeeGroupsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.EmployeesGroup> _EmployeeGroupsQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.EmployeesGroup> _EmployeeGroupsCommand;

        public DeleteEmployeeGroups(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.EmployeesGroup> EmployeeGroupsQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.EmployeesGroup> EmployeeGroupsCommand)
        {
            _EmployeeGroupsQuery = EmployeeGroupsQuery;
            _EmployeeGroupsCommand = EmployeeGroupsCommand;
        }

        public async Task<ResponseResult> Handle(DeleteEmployeeGroupsRequest request, CancellationToken cancellationToken)
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
            var elemetsForDelete = _EmployeeGroupsQuery.TableNoTracking.Include(c => c.InvEmployees).Where(c => ids.Contains(c.Id) && !c.InvEmployees.Any());
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

            _EmployeeGroupsCommand.RemoveRange(elemetsForDelete);
            var saved = await _EmployeeGroupsCommand.SaveChanges() > 0 ? true : false;

            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
