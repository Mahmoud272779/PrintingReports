using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments
{
    public static class SectionsAndDepartmentsHelper
    {
        public static ResponseResult isValid(int empId, int parentId, SectionsAndDepartmentsType type, IRepositoryQuery<InvEmployees> _InvEmployeesQuery, IRepositoryQuery<GLBranch> _GLBranchQuery, IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _sectionsAndDepartmentsQuery)
        {
            //check employee
            if (!_InvEmployeesQuery.TableNoTracking.Any(c => c.Id == empId) && empId!=0)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        type = Enums.AlartShow.popup,
                        AlartType = Enums.AlartType.error,
                        MessageAr = "هذا الموظف غير موجود",
                        MessageEn = "This Employee is not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    },
                };
            if (type == SectionsAndDepartmentsType.Departments)
            {
                if (!_GLBranchQuery.TableNoTracking.Any(c => c.Id == parentId))
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            type = Enums.AlartShow.popup,
                            AlartType = Enums.AlartType.error,
                            MessageAr = "هذا الفرع غير موجود",
                            MessageEn = "This branch is not exist",
                            titleAr = "خطأ",
                            titleEn = "Error"
                        },
                    };

            }
            else if (type == SectionsAndDepartmentsType.Sections)
            {
                if (!_sectionsAndDepartmentsQuery.TableNoTracking.Any(c => c.Id == parentId))
                    return new ResponseResult
                    {
                        Result = Result.Failed,
                        Alart = new Alart
                        {
                            type = Enums.AlartShow.popup,
                            AlartType = Enums.AlartType.error,
                            MessageAr = "هذه الادارة غير موجود",
                            MessageEn = "This Department is not exist",
                            titleAr = "خطأ",
                            titleEn = "Error"
                        },
                    };
            }
            else
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        type = Enums.AlartShow.popup,
                        AlartType = Enums.AlartType.error,
                        MessageAr = "خطا في اختيار نوع الاضافة",
                        MessageEn = "The Type is not correct",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    },
                };
            return null;
        }
    }
}
