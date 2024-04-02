using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Infrastructure;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.ChangefulTime_AddEmps
{
    public class ChangefulTime_EditEmpsHandler : IRequestHandler<ChangefulTime_EditEmpsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _invEmployeesQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> _ChangefulTimeGroupsMasterQuery;
        private readonly IRepositoryCommand<ChangefulTimeGroupsEmployees> _ChangefulTimeGroupsEmployeesCommand;
        private readonly IRepositoryQuery<ChangefulTimeGroupsEmployees> _ChangefulTimeGroupsEmployeesQuery;
        public ChangefulTime_EditEmpsHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryCommand<ChangefulTimeGroupsEmployees> changefulTimeGroupsEmployeesCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.ChangefulTimeGroupsMaster> changefulTimeGroupsMasterQuery, IRepositoryQuery<ChangefulTimeGroupsEmployees> changefulTimeGroupsEmployeesQuery)
        {
            _invEmployeesQuery = invEmployeesQuery;
            _ChangefulTimeGroupsEmployeesCommand = changefulTimeGroupsEmployeesCommand;
            _ChangefulTimeGroupsMasterQuery = changefulTimeGroupsMasterQuery;
            _ChangefulTimeGroupsEmployeesQuery = changefulTimeGroupsEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(ChangefulTime_EditEmpsRequest request, CancellationToken cancellationToken)
        {
            var CheckGroupExist = await  _ChangefulTimeGroupsMasterQuery.GetByIdAsync(request.groupId);
            if (CheckGroupExist == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        MessageAr = "هذه المجموعه غير موجوده",
                        MessageEn = "This group is not exist",
                        titleAr = "خطأ",
                        titleEn = "Error",
                        type = AlartShow.popup
                    }
                };
            var ExistEmplyees = _ChangefulTimeGroupsEmployeesQuery.TableNoTracking.Where(c => c.changefulTimeGroupsMasterId == request.groupId);
            if (ExistEmplyees.Any())
            {
                _ChangefulTimeGroupsEmployeesCommand.RemoveRange(ExistEmplyees);
                await _ChangefulTimeGroupsEmployeesCommand.SaveAsync();
            }
            var empIds = request.employeesId.Split(',').Select(x => int.Parse(x)).ToArray();
            var selectEmployees = _invEmployeesQuery.TableNoTracking.Where(c => empIds.Contains(c.Id));
            var listOf_ChangefulTimeGroupsEmployees = new List<ChangefulTimeGroupsEmployees>();
            foreach (var item in selectEmployees)
            {
                listOf_ChangefulTimeGroupsEmployees.Add(new ChangefulTimeGroupsEmployees
                {
                    changefulTimeGroupsMasterId = request.groupId,
                    invEmployeesId = item.Id
                });
            }
            _ChangefulTimeGroupsEmployeesCommand.AddRange(listOf_ChangefulTimeGroupsEmployees);
            var saved = await _ChangefulTimeGroupsEmployeesCommand.SaveAsync();
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
