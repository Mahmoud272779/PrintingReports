using App.Application.Handlers.AttendLeaving.HolidaysEmployees.AddHolidaysEmployees;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.Application.Handlers.AttendLeaving.EmployeeGroups.AddEmployeesInGroup
{
    public class AddEmployeesInGroupHandler : IRequestHandler<AddEmployeesInGroupRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryCommand<InvEmployees> _InvEmployeesCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.EmployeesGroup> _EmployeesGroupQuery;

        public AddEmployeesInGroupHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.EmployeesGroup> employeesGroupQuery, IRepositoryCommand<InvEmployees> invEmployeesCommand)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _EmployeesGroupQuery = employeesGroupQuery;
            _InvEmployeesCommand = invEmployeesCommand;
        }


        public async Task<ResponseResult> Handle(AddEmployeesInGroupRequest request, CancellationToken cancellationToken)
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
            var checkGroup = await _EmployeesGroupQuery.GetByIdAsync(request.parentId);
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
                
                item.employeesGroupId = request.parentId;
                

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
