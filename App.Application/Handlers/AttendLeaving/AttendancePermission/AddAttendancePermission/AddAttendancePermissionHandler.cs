using App.Application.Handlers.AttendLeaving.Holidays.AddHolidays;
using App.Domain.Models.Security.Authentication.Response.Store;
using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.AddAttendancePermission
{
    public class AddAttendancePermissionHandler : IRequestHandler<AddAttendancePermissionRequset, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionCommand;

        public AddAttendancePermissionHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.AttendancPermission> attendancPermissionCommand)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _AttendancPermissionCommand = attendancPermissionCommand;
        }

        public async Task<ResponseResult> Handle(AddAttendancePermissionRequset request, CancellationToken cancellationToken)
        {
            var allEmps = _InvEmployeesQuery.TableNoTracking.Include(e=>e.shiftsMaster).ToList();
            var checkEmpId = allEmps.Where(e => e.Id == request.EmpId).FirstOrDefault();
            if (checkEmpId == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = "الموظف غير موجود",
                        MessageEn = "This Emp does not exist",
                    }
                };
            if(request.shift1_start>request.shift1_end || request.shift2_start > request.shift2_end ||
                request.shift3_start > request.shift3_end || request.shift4_start > request.shift4_end)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.StartDateAfterEndDate,
                        MessageEn = ErrorMessagesEn.StartDateAfterEndDate,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            if (request.type != null) 
            {
                
                if (request.type == (int)PermissionTypeEnum.Day)
                {
                    var saved = await _AttendancPermissionCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.AttendancPermission
                    {
                        EmpId = request.EmpId,
                        Day = request.Day,
                        type = request.type,

                    });

                    return new ResponseResult
                    {
                        Result = saved ? Result.Success : Result.Failed,
                        Alart = saved ? new Alart
                        {
                            AlartType = AlartType.success,
                            type = AlartShow.note,
                            MessageAr = ErrorMessagesAr.SaveSuccessfully,
                            MessageEn = ErrorMessagesEn.SaveSuccessfully,
                            titleAr = "save",
                            titleEn = "save"
                        } :
                     new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
                    };
                }

                else if (request.type == (int)PermissionTypeEnum.Temp)
                {
                    if (checkEmpId.shiftsMaster!=null) 
                    {
                        if (checkEmpId.shiftsMaster.shiftType == (int)shiftTypes.normal)
                        {
                            var saved = await _AttendancPermissionCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.AttendancPermission
                            {
                                EmpId = request.EmpId,
                                Day = request.Day,
                                type = request.type,
                                totalHoursForOpenShift = request.totalHoursForOpenShift,

                            });

                            return new ResponseResult
                            {
                                Result = saved ? Result.Success : Result.Failed,
                                Alart = saved ? new Alart
                                {
                                    AlartType = AlartType.success,
                                    type = AlartShow.note,
                                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                                    MessageEn = ErrorMessagesEn.SaveSuccessfully,
                                    titleAr = "save",
                                    titleEn = "save"
                                } :
                         new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
                            };
                        }

                        else if (checkEmpId.shiftsMaster.shiftType == (int)shiftTypes.openShift || checkEmpId.shiftsMaster.shiftType == (int)shiftTypes.ChangefulTime)
                        {
                            var saved = await _AttendancPermissionCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.AttendancPermission
                            {
                                EmpId = request.EmpId,
                                Day = request.Day,
                                type = request.type,
                                shift1_start = request.shift1_start,
                                shift2_start = request.shift2_start,
                                shift3_start = request.shift3_start,
                                shift4_start = request.shift4_start,
                                shift1_end = request.shift1_end,
                                shift2_end = request.shift2_end,
                                shift3_end = request.shift3_end,
                                shift4_end = request.shift4_end,
                            });

                            return new ResponseResult
                            {
                                Result = saved ? Result.Success : Result.Failed,
                                Alart = saved ? new Alart
                                {
                                    AlartType = AlartType.success,
                                    type = AlartShow.note,
                                    MessageAr = ErrorMessagesAr.SaveSuccessfully,
                                    MessageEn = ErrorMessagesEn.SaveSuccessfully,
                                    titleAr = "save",
                                    titleEn = "save"
                                } :
                         new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
                            };
                        }
                    }
                }


            }


            return new ResponseResult
            {
                Result = Result.Failed,
                Alart = new Alart
                {
                    AlartType = AlartType.error,
                    type = AlartShow.note,
                    MessageAr = "خطا اثناء معالجة الطلب",
                    MessageEn = "error while handling the request",
                }
            };



        }
    }
}
