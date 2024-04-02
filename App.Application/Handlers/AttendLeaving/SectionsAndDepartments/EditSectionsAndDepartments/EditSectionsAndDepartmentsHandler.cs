using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.EditSectionsAndDepartments
{
    public class EditSectionsAndDepartmentsHandler : IRequestHandler<EditSectionsAndDepartmentsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _sectionsAndDepartmentsQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;
        private readonly IRepositoryQuery<GLBranch> _GLBranchQuery;
        private readonly IRepositoryCommand<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _sectionsAndDepartmentsCommand;
        public EditSectionsAndDepartmentsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsQuery, IRepositoryQuery<InvEmployees> invEmployeesQuery, IRepositoryQuery<GLBranch> gLBranchQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsCommand)
        {
            _sectionsAndDepartmentsQuery = sectionsAndDepartmentsQuery;
            _InvEmployeesQuery = invEmployeesQuery;
            _GLBranchQuery = gLBranchQuery;
            _sectionsAndDepartmentsCommand = sectionsAndDepartmentsCommand;
        }
        public async Task<ResponseResult> Handle(EditSectionsAndDepartmentsRequest request, CancellationToken cancellationToken)
        {
            var element = _sectionsAndDepartmentsQuery.TableNoTracking.FirstOrDefault(c => c.Id == request.Id);
            if (element == null)
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        type = Enums.AlartShow.note,
                        AlartType = Enums.AlartType.success,
                        MessageAr = request.Type == SectionsAndDepartmentsType.Departments ?  "هذه الادارة غير موجوده":"هذا القسم غير موجود",
                        MessageEn = request.Type == SectionsAndDepartmentsType.Departments ?  "This Department is not exist" : "This Section is not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            var IsValid = SectionsAndDepartmentsHelper.isValid(empId: request.empId ?? 0, parentId: request.Id, type: request.Type, _InvEmployeesQuery: _InvEmployeesQuery, _GLBranchQuery: _GLBranchQuery, _sectionsAndDepartmentsQuery: _sectionsAndDepartmentsQuery);
            if (IsValid != null)
                return IsValid;
            if(string.IsNullOrEmpty(request.arabicName.Trim()))
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

            element.arabicName = request.arabicName.Trim();
            element.latinName = request.latinName.Trim();
            element.empId = request.empId;
            var saved = await _sectionsAndDepartmentsCommand.UpdateAsyn(element);
            if (saved)
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
