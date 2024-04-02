using App.Application.Handlers.AttendLeaving.CalculatingWorkingHours.TimeCalculation;
using App.Application.Handlers.AttendLeaving.Holidays.AddHolidays;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.GetShiftDetailsForEmpIdDay;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Models.Security.Authentication.Response.Store;
using App.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission.AddAttendancePermission
{

    public class AddAttendancePermissionHandler : IRequestHandler<AddAttendancePermissionRequset, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery;
        private readonly IMediator _mediator;

        public AddAttendancePermissionHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.AttendancPermission> attendancPermissionCommand, IMediator mediator, IRepositoryQuery<AttendancPermission> attendancPermissionQuery)
        {
            _InvEmployeesQuery = invEmployeesQuery;
            _AttendancPermissionCommand = attendancPermissionCommand;
            _mediator = mediator;
            _AttendancPermissionQuery = attendancPermissionQuery;
        }

        public async Task<ResponseResult> Handle(AddAttendancePermissionRequset request, CancellationToken cancellationToken)
        {


            if (!(request.type == (int)Enums.PermissionTypeEnum.Temp || request.type == (int)Enums.PermissionTypeEnum.Day))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "يجب عليك إدخال نوع الإذن 1 لليوم، 2 للمؤقت",
                        MessageEn = "You must enter permission type 1 for day, 2 for temp",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }

                };


            var allEmps = _InvEmployeesQuery.TableNoTracking.Include(e => e.shiftsMaster).ToList();
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
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };

            var AddPermission_ShiftsTimeForNormalAndChangfulShift = new AddPermission_ShiftsTime();
            if (checkEmpId.shiftsMaster.shiftType != (int)Enums.shiftTypes.openShift)
            {
                var isValied = AttendancePermissionHelper.isValied(request);
                if (isValied.Result != Result.Success)
                    return isValied;
                AddPermission_ShiftsTimeForNormalAndChangfulShift = (AddPermission_ShiftsTime)isValied.Data;
            }
            else
            {
                if (request.totalHoursForOpenShift == null || request.totalHoursForOpenShift == TimeSpan.Zero)
                {
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            AlartType = AlartType.error,
                            type = AlartShow.popup,
                            MessageAr = "يجب ادخال عدد ساعات العمل",
                            MessageEn = "You have to enter Total hours for shift",
                            titleAr = "خطأ",
                            titleEn = "Error"
                        }
                    };
                }

            }




            if ((!request.shift1_IsExtended && request.shift1_start > request.shift1_end) || (!request.shift2_IsExtended && request.shift2_start > request.shift2_end) ||
           (!request.shift3_IsExtended && request.shift3_start > request.shift3_end) || (!request.shift4_IsExtended && request.shift4_start > request.shift4_end))
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


            if (checkEmpId.shiftsMaster == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "الموظف غير مسجل على دوام",
                        MessageEn = "Emp has no shift",

                    }

                };

            bool saved = false;

            var element = new AttendancPermission();
            if (request.isUpdate)
            {
                element = _AttendancPermissionQuery.TableNoTracking.FirstOrDefault(c => c.Id == request.Id);
                if(element == null)
                {
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            AlartType = AlartType.error,
                            type = AlartShow.popup,
                            MessageAr = "هذا العنصر غير موجود",
                            MessageEn = "This Element is not exist",
                            titleAr ="خطأ",
                            titleEn = "Error"
                        }
                    };
                }
            }
            if (request.type == (int)PermissionTypeEnum.Day)
            {
                element = new Domain.Entities.Process.AttendLeaving.AttendancPermission
                {
                    Id = request.isUpdate ? element.Id : 0,
                    EmpId = request.EmpId,
                    Day = request.Day,
                    type = request.type,
                    note = request.Note ?? "",
                    IsMoved = false,

                };
            }
            else if (request.type == (int)PermissionTypeEnum.Temp)
            {
                if (checkEmpId.shiftsMaster != null)
                {
                    if (checkEmpId.shiftsMaster.shiftType == (int)shiftTypes.normal)
                    {
                        element = new AttendancPermission
                        {
                            Id = request.isUpdate ? element.Id : 0,
                            EmpId = request.EmpId,
                            Day = request.Day,
                            type = request.type,

                            shift1_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift1Start,
                            shift1_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift1End,

                            haveShift2 = request.Has_shift2,
                            shift2_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift2Start,
                            shift2_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift2End,

                            haveShift3 = request.Has_shift3,
                            shift3_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift3Start,
                            shift3_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift3End,

                            haveShift4 = request.Has_shift4,
                            shift4_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift4Start,
                            shift4_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift4End,
                            note = request.Note,
                            IsMoved = false,
                            isShift1_extended = request.shift1_IsExtended,
                            isShift2_extended = request.shift2_IsExtended,
                            isShift3_extended = request.shift3_IsExtended,
                            isShift4_extended = request.shift4_IsExtended,

                        };
                    }

                    else if (checkEmpId.shiftsMaster.shiftType == (int)shiftTypes.ChangefulTime)
                    {
                        element = new AttendancPermission
                        {
                            Id = request.isUpdate ? element.Id : 0,

                            EmpId = request.EmpId,
                            Day = request.Day,
                            type = request.type,

                            shift1_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift1Start,
                            shift1_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift1End,

                            haveShift2 = request.Has_shift2,
                            shift2_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift2Start,
                            shift2_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift2End,

                            haveShift3 = request.Has_shift3,
                            shift3_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift3Start,
                            shift3_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift3End,

                            haveShift4 = request.Has_shift4,
                            shift4_start = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift4Start,
                            shift4_end = AddPermission_ShiftsTimeForNormalAndChangfulShift.shift4End,

                            note = request.Note,
                            IsMoved = false,
                            isShift1_extended = request.shift1_IsExtended,
                            isShift2_extended = request.shift2_IsExtended,
                            isShift3_extended = request.shift3_IsExtended,
                            isShift4_extended = request.shift4_IsExtended,
                        };

                    }

                    else if (checkEmpId.shiftsMaster.shiftType == (int)shiftTypes.openShift)
                    {
                        element = new   AttendancPermission
                        {
                            Id = request.isUpdate ? element.Id : 0,

                            EmpId = request.EmpId,
                            Day = request.Day,
                            type = request.type,
                            totalHoursForOpenShift = request.totalHoursForOpenShift,
                            note = request.Note,
                            IsMoved = false,


                        };
                    }
                }



            }
            saved = request.isUpdate ? await _AttendancPermissionCommand.UpdateAsyn(element) : await _AttendancPermissionCommand.AddAsync(element);



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
                    new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.ErrorSaving,
                        MessageEn = ErrorMessagesEn.ErrorSaving,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
            };





        }
    }
}
