using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.ChangefulTime.EditChangefulTimeDays;
using App.Domain.Entities.Process.AttendLeaving;
using App.Domain.Entities.Process.AttendLeaving.Shift;
using App.Infrastructure;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using MediatR;

namespace App.Application.Handlers.AttendLeaving.Shifts.ShiftDays.GetShiftDetailsForEmpIdDay
{
    public class GetShiftDetailsForEmpIdDayHandler : IRequestHandler<GetShiftDetailsForEmpIdDayRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<InvEmployees> _invEmployeesQuery;

        private readonly IRepositoryQuery<ChangefulTimeDays> _ChangefulTimeDaysQuery;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.AttendancPermission> _AttendancPermissionQuery;
        public GetShiftDetailsForEmpIdDayHandler(IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<NormalShiftDetalies> normalShiftDetaliesQuery, IRepositoryQuery<ChangefulTimeDays> changefulTimeDaysQuery, IRepositoryQuery<AttendancPermission> attendancPermissionQuery)
        {
            _invEmployeesQuery = invEmployeesQuery;

            _ChangefulTimeDaysQuery = changefulTimeDaysQuery;
            _AttendancPermissionQuery = attendancPermissionQuery;
        }


        public async Task<ResponseResult> Handle(GetShiftDetailsForEmpIdDayRequest request, CancellationToken cancellationToken)
        {

            // check day and empId in database exists or not 
            var permission = _AttendancPermissionQuery.TableNoTracking.FirstOrDefault(c => c.Day.Date == request.day.Date && c.EmpId == request.EmpId);

            if (permission != null && !request.isEdited)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "الموظف بالفعل لديه اذن فى ذلك اليوم ",
                        MessageEn = "Employee already has permission in this day",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }

                };
            var emp = _invEmployeesQuery
                .TableNoTracking
                .Include(e => e.shiftsMaster)
                .Include(e => e.shiftsMaster.normalShiftDetalies)
                .Include(e => e.shiftsMaster.changefulTimeGroups)
                .FirstOrDefault(c => c.Id == request.EmpId);

            if (emp == null)
            {
                return new ResponseResult // employee not exist
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "الموظف غير موجود",
                        MessageEn = "Emp not exist",

                    }


                };
            }

            shifts shifts = new shifts();
            var editedshifts = new shifts();
            shiftsDTO shiftsDTO_Response = new shiftsDTO();
            ChangefulTimeDays changefulTimeDays = new ChangefulTimeDays();
            NormalShiftDetalies normalShiftDetalies = new NormalShiftDetalies();

            if(emp.shiftsMaster == null)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr ="هذا الموظف غير مسجل علي دوام",
                        MessageEn ="This employee has no shift",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            }    


            if (!request.isEdited)
            {
                shiftsDTO_Response = new shiftsDTO
                {
                    shifts = shifts,
                    isVacation = emp.shiftsMaster.shiftType == (int)shiftTypes.normal ? emp.shiftsMaster.normalShiftDetalies.FirstOrDefault(c => c.DayId == Lists.days.FirstOrDefault(c => c.latinName == request.day.DayOfWeek.ToString()).Id)?.IsVacation : changefulTimeDays.IsVacation,
                    shiftType = emp.shiftsMaster.shiftType
                };


            }
            else
            {
                var element = await _AttendancPermissionQuery.GetByIdAsync(request.id);
                if (element == null)
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            AlartType = AlartType.error,
                            type = AlartShow.popup,
                            MessageAr = ErrorMessagesAr.ThisElementIsNotExist,
                            MessageEn = ErrorMessagesEn.ThisElementIsNotExist,
                            titleAr = "خطأ",
                            titleEn = "Error"
                        }
                    };
                editedshifts = new shifts
                {
                    shift1_start = element.shift1_start?.TimeOfDay.ToString("hh\\:mm") ?? null,
                    shift1_end = element.shift1_end?.TimeOfDay.ToString("hh\\:mm") ?? null,


                    shift2_start = element.shift2_start?.TimeOfDay.ToString("hh\\:mm") ?? null,
                    shift2_end = element.shift2_end?.TimeOfDay.ToString("hh\\:mm") ?? null,



                    shift3_start = element.shift3_start?.TimeOfDay.ToString("hh\\:mm") ?? null,
                    shift3_end = element.shift3_end?.TimeOfDay.ToString("hh\\:mm") ?? null,



                    shift4_start = element.shift4_start?.TimeOfDay.ToString("hh\\:mm") ?? null,
                    shift4_end = element.shift4_end?.TimeOfDay.ToString("hh\\:mm") ?? null,

                    TotalDayHours = element.totalHoursForOpenShift?.ToString("hh\\:mm") ?? null,

                    Has_shift2 = element.shift2_start != null && element.shift2_end != null ? true : false,
                    Has_shift3 = element.shift3_start != null && element.shift3_end != null ? true : false,
                    Has_shift4 = element.shift4_start != null && element.shift4_end != null ? true : false,

                    shift1_IsExtended = element.isShift1_extended,
                    shift2_IsExtended = element.isShift2_extended,
                    shift3_IsExtended = element.isShift3_extended,
                    shift4_IsExtended = element.isShift4_extended,
                };

                shiftsDTO_Response = new shiftsDTO
                {
                    shifts = shifts,
                    Editedshifts = editedshifts,
                    isVacation = emp.shiftsMaster.shiftType == (int)shiftTypes.normal ? emp.shiftsMaster.normalShiftDetalies.FirstOrDefault(c => c.DayId == Lists.days.FirstOrDefault(c => c.latinName == request.day.DayOfWeek.ToString()).Id).IsVacation : changefulTimeDays.IsVacation,
                    shiftType = emp.shiftsMaster.shiftType,
                };

            }




            return new ResponseResult
            {
                Data = shiftsDTO_Response,
                Result = Result.Success
            };

        }
    }
    public class shiftsDTO
    {
        public bool? isVacation { get; set; }
        public shifts shifts { get; set; }

        public shifts Editedshifts { get; set; }
        public int? shiftType { get; set; }
    }
    public class shifts
    {
        public string shift1_start { get; set; }
        public string shift1_end { get; set; }
        public bool shift1_IsExtended { get; set; }
        public string shift2_start { get; set; }
        public string shift2_end { get; set; }
        public bool shift2_IsExtended { get; set; }
        public string shift3_start { get; set; }
        public string shift3_end { get; set; }
        public bool shift3_IsExtended { get; set; }
        public string shift4_start { get; set; }
        public string shift4_end { get; set; }
        public bool shift4_IsExtended { get; set; }


        public bool Has_shift2 { get; set; }
        public bool Has_shift3 { get; set; }
        public bool Has_shift4 { get; set; }

        public string TotalDayHours { get; set; }

    }
}
