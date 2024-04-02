using MediatR;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.AddSectionsAndDepartments
{
    public class AddSectionsAndDepartmentsHandler : IRequestHandler<AddSectionsAndDepartmentsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _sectionsAndDepartmentsQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _sectionsAndDepartmentsCommand;
        public AddSectionsAndDepartmentsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsCommand, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery)
        {
            _sectionsAndDepartmentsQuery = sectionsAndDepartmentsQuery;
            _sectionsAndDepartmentsCommand = sectionsAndDepartmentsCommand;
            _InvEmployeesQuery = invEmployeesQuery;
            _GLBranchQuery = gLBranchQuery;
        }
        /*###############   NOTE   ################*/
        // 0 for Departments,1 for Sections
        //Departments parent is BranchId --- Section Parent is DepartmentsId
        /*###############   NOTE   ################*/

        public async Task<ResponseResult> Handle(AddSectionsAndDepartmentsRequest request, CancellationToken cancellationToken)
        {
            //Check Is valied
            var IsValid = SectionsAndDepartmentsHelper.isValid(empId:request.empId??0,parentId:request.parentId,type:request.Type,_InvEmployeesQuery:_InvEmployeesQuery,_GLBranchQuery:_GLBranchQuery,_sectionsAndDepartmentsQuery:_sectionsAndDepartmentsQuery);
            if (IsValid != null)
                return IsValid;

            //Generate NextCode
            var nextCode = _sectionsAndDepartmentsQuery.TableNoTracking.Where(c => c.Type == (int)request.Type).Count();
            if (nextCode == 0)
                nextCode = 1;
            else
                nextCode = nextCode + 1;

            if (string.IsNullOrEmpty(request.arabicName.Trim()))
                return new ResponseResult
                {
                    Result = Result.Success,
                    Alart = new Alart
                    {
                        type = Enums.AlartShow.note,
                        AlartType = Enums.AlartType.success,
                        MessageAr = "الاسم العربي مطلوب",
                        MessageEn = "Arabic name is required",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            if (string.IsNullOrEmpty(request.latinName.Trim()))
                request.latinName = request.arabicName.Trim();


            //Add to database
            App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments table = new App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments
            {
                code = nextCode,
                Type = (int)request.Type,
                arabicName = request.arabicName.Trim(),
                latinName = request.latinName.Trim(),
                empId = request.empId,
                parentId = request.parentId
            };
            var Added = await _sectionsAndDepartmentsCommand.AddAsync(table);
            if (Added)
                return new ResponseResult
                {
                    Result = Result.Success,
                    Alart = new Alart
                    {
                        type = Enums.AlartShow.note,
                        AlartType = Enums.AlartType.success,
                        MessageAr = "تم الحفظ بنجاح",
                        MessageEn = "Saved successfully",
                        titleAr = "حفظ",
                        titleEn = "Save"
                    }
                };
            else
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        type = Enums.AlartShow.popup,
                        AlartType = Enums.AlartType.error,
                        MessageAr = "حدث خطأ أثناء الحفظ",
                        MessageEn = "Error while saving",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };

        }
        
    }
}
