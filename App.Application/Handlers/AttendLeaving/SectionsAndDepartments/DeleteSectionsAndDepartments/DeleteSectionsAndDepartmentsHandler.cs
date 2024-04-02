using App.Application.Handlers.Setup.ItemCard.Command;
using App.Infrastructure;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.SectionsAndDepartments.DeleteSectionsAndDepartments
{
    public class DeleteSectionsAndDepartmentsHandler : IRequestHandler<DeleteSectionsAndDepartmentsRequest, ResponseResult>
    {
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _SectionsAndDepartmentsQuery;
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> _SectionsAndDepartmentsCommand;
        public DeleteSectionsAndDepartmentsHandler(IRepositoryQuery<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsQuery, IRepositoryCommand<Domain.Entities.Process.AttendLeaving.SectionsAndDepartments> sectionsAndDepartmentsCommand)
        {
            _SectionsAndDepartmentsQuery = sectionsAndDepartmentsQuery;
            _SectionsAndDepartmentsCommand = sectionsAndDepartmentsCommand;
        }

        public async Task<ResponseResult> Handle(DeleteSectionsAndDepartmentsRequest request, CancellationToken cancellationToken)
        {
            var ids = request.Id.Split(',').Select(c => int.Parse(c)).ToList();
            var Elements = _SectionsAndDepartmentsQuery
                .TableNoTracking
                .Include(c=> c.invEmployeesSection)
                .Include(c=> c.invEmployeesDepartment)
                .Where(c => c.Type == (int)request.Type && ids.Contains(c.Id) && !c.invEmployeesSection.Any() && !c.invEmployeesDepartment.Any());
            if (!Elements.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        type = Enums.AlartShow.popup,
                        AlartType = Enums.AlartType.error,
                        MessageAr = request.Type == SectionsAndDepartmentsType.Departments ? "هذه الادارة غير موجود" : "هذه الاقسام غير موجودة",
                        MessageEn = request.Type == SectionsAndDepartmentsType.Departments ? "This Department is not exist" : "This Section is not exist",
                        titleAr = "خطأ",
                        titleEn = "Error"
                    },
                };
             _SectionsAndDepartmentsCommand.RemoveRange(Elements);
            var deleted = await _SectionsAndDepartmentsCommand.SaveChanges() > 0 ? true : false ;
            return new ResponseResult
            {
                Result = deleted ? Result.Success : Result.Failed,
                Alart = new Alart
                {
                    type = deleted ? Enums.AlartShow.note : Enums.AlartShow.popup,
                    AlartType = deleted ? Enums.AlartType.success : Enums.AlartType.error,
                    MessageAr = deleted ? "تم الحذف بنجاح" : ErrorMessagesAr.ErrorSaving,
                    MessageEn = deleted ?  "Delete successfully" : ErrorMessagesEn.ErrorSaving,
                    titleAr = deleted ?"": "خطأ",
                    titleEn = deleted ?"": "Error"
                },
            };
        }
    }
}
