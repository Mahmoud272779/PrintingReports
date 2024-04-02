using App.Application.Handlers.AttendLeaving.AttendancePermission.AddAttendancePermission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.AttendancePermission
{
    public static class AttendancePermissionHelper
    {
        public static ResponseResult isValied(AddAttendancePermissionRequset request)
        {
            bool[] shiftsExtended = { request.shift1_IsExtended, request.shift2_IsExtended, request.shift4_IsExtended };
            if (shiftsExtended.Count(c => c == true) > 1)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "لا يمكن ان يكون عدد الدوامات الممتده اكتر من 1",
                        MessageEn = "You can not make more than 1 shift extended",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            }
            if (request.shift1_start == null || request.shift1_end == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "يجب ادخال قيمه للدوام رقم 1",
                        MessageEn = "You must enter value for shift 1",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };





            //shifts times 
            DateTime shift1Start = request.Day.Date.AddHours(request.shift1_start.Value.TotalHours);
            DateTime shift1End = request.Day.Date.AddDays(request.shift1_IsExtended ? 1 : 0).AddHours(request.shift1_end.Value.TotalHours);

            DateTime shift2Start = request.Has_shift2 ? request.Day.Date.AddDays(request.shift1_IsExtended ? 1 : 0).AddHours(request.shift2_start.Value.TotalHours) : DateTime.Now;
            DateTime shift2End = request.Has_shift2 ? request.Day.Date.AddDays(request.shift1_IsExtended || request.shift2_IsExtended ? 1 : 0).AddHours(request.shift2_end.Value.TotalHours) : DateTime.Now;

            DateTime shift3Start = request.Has_shift3 ? request.Day.Date.AddDays(request.shift2_IsExtended ? 1 : 0).AddHours(request.shift3_start.Value.TotalHours) : DateTime.Now;
            DateTime shift3End = request.Has_shift3 ? request.Day.Date.AddDays(request.shift2_IsExtended || request.shift3_IsExtended ? 1 : 0).AddHours(request.shift3_end.Value.TotalHours) : DateTime.Now;

            DateTime shift4Start = request.Has_shift4 ? request.Day.Date.AddDays(request.shift3_IsExtended ? 1 : 0).AddHours(request.shift4_start.Value.TotalHours) : DateTime.Now;
            DateTime shift4End = request.Has_shift4 ? request.Day.Date.AddDays(request.shift3_IsExtended || request.shift4_IsExtended ? 1 : 0).AddHours(request.shift4_end.Value.TotalHours) : DateTime.Now;


            AddPermission_ShiftsTime AddPermission_ShiftsTime_res = new AddPermission_ShiftsTime
            {
                shift1Start = shift1Start,
                shift1End = shift1End,
                shift2Start = request.Has_shift2 ? shift2Start : null,
                shift2End = request.Has_shift2 ? shift2End : null,
                shift3Start = request.Has_shift3 ? shift3Start : null,
                shift3End = request.Has_shift3 ? shift3End : null,
                shift4Start = request.Has_shift4 ? shift4Start : null,
                shift4End = request.Has_shift4 ? shift4End : null,

            };

            var response = new ResponseResult();

            //shift 1
            response = checkShiftsValidation(shift1Start, shift1End, 1, AddPermission_ShiftsTime_res);
            if (response != null)
                return response;

            if (request.Has_shift2)
            {
                response = checkShiftsValidation(shift2Start, shift2End, 2, AddPermission_ShiftsTime_res);
                if (response != null)
                    return response;
            }
            if (request.Has_shift3)
            {
                response = checkShiftsValidation(shift3Start, shift3End, 3, AddPermission_ShiftsTime_res);
                if (response != null)
                    return response;
            }
            if (request.Has_shift4)
            {
                response = checkShiftsValidation(shift4Start, shift4End, 4, AddPermission_ShiftsTime_res);
                if (response != null)
                    return response;
            }


            return new ResponseResult
            {
                Data = AddPermission_ShiftsTime_res,
                Result = Result.Success
            };
        }
        public static ResponseResult checkShiftsValidation(DateTime shiftStart, DateTime shiftEnd, int shiftNumber, AddPermission_ShiftsTime shiftsTimes)
        {
            if (shiftStart == shiftEnd)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = $"لا يمكن ان يتساوي وقت الدخول و وقت الخروج في الدوام رقم {shiftNumber}",
                        MessageEn = $"Shift {shiftNumber} start and shift {shiftNumber} end can not be equeled",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            }
            if (shiftStart > shiftEnd)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = $"لا يمكن بدايه الدوام رقم {shiftNumber} ان تكون اقل من نهايته",
                        MessageEn = $"Shift {shiftNumber} start and shift {shiftNumber} end can not be equeled",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            }

            if (shiftsTimes.shift2Start < shiftsTimes.shift1End)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "بدايه الدوام رقم 2 لا يكون ان تكون اقل من نهايه الدوام رقم 1",
                        MessageEn = "The start of shift 2 can not be less than end of shift 1 ",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            }
            if (shiftsTimes.shift3Start < shiftsTimes.shift2End)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "بدايه الدوام رقم 3 لا يكون ان تكون اقل من نهايه الدوام رقم 2",
                        MessageEn = "The start of shift 3 can not be less than end of shift 2 ",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            }
            if (shiftsTimes.shift4Start < shiftsTimes.shift3End)
            {
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = "بدايه الدوام رقم 4 لا يكون ان تكون اقل من نهايه الدوام رقم 3",
                        MessageEn = "The start of shift 4 can not be less than end of shift 3 ",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            }

            return null;

        }






    }
    public class AddPermission_ShiftsTime
    {
        public DateTime? shift1Start { get; set; }
        public DateTime? shift1End { get; set; }
        public DateTime? shift2Start { get; set; }
        public DateTime? shift2End { get; set; }
        public DateTime? shift3Start { get; set; }
        public DateTime? shift3End { get; set; }
        public DateTime? shift4Start { get; set; }
        public DateTime? shift4End { get; set; }
    }
}
