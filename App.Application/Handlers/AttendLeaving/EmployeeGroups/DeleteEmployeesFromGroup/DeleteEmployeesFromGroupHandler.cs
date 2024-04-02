using App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeesInGroup;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.DeleteEmployeesFromGroup
{
    public class DeleteEmployeesFromGroupHandler : IRequestHandler<DeleteEmployeesFromGroupRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryCommand<InvEmployees> _InvEmployeesCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.EmployeesGroup> _EmployeesGroupQuery;
        public DeleteEmployeesFromGroupHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryCommand<InvEmployees> invEmployeesCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.EmployeesGroup> employeesGroupQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _InvEmployeesCommand = invEmployeesCommand;
            _EmployeesGroupQuery = employeesGroupQuery;
        }


        public async Task<ResponseResult> Handle(DeleteEmployeesFromGroupRequest request, CancellationToken cancellationToken)
        {
            int[] empsId = null;
            if (!string.IsNullOrEmpty(request.empIds))
                empsId = request.empIds.Split(',').Select(c => int.Parse(c)).ToArray();
            if (empsId == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "يجب اختيار موظفين",
                        MessageEn = "You have to choose employees",
                    }
                };
            var checkGroup = await _EmployeesGroupQuery.GetByIdAsync(request.groupId);
            if (checkGroup == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "هذه المجموهة غير موجوده",
                        MessageEn = "This Group does not exist",
                    }
                };

            var emps = _InvEmployeesQuery.TableNoTracking.Where(c => empsId.Contains(c.Id)).ToList();

            foreach (var item in emps)
            {
                item.employeesGroupId = null;
            }




            var saved = await _InvEmployeesCommand.UpdateAsyn(emps);
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };


        }
    }
}
