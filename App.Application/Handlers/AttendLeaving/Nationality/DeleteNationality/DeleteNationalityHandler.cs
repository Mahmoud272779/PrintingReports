using App.Infrastructure;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.AttendLeaving.Nationality.DeleteNationality
{
    public class DeleteNationalityHandler : IRequestHandler<DeleteNationalityRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Nationality> _NationalityCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Nationality> _NationalityQuery;
        private readonly IRepositoryQuery<InvEmployees> _InvEmployeesQuery;

        public DeleteNationalityHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Nationality> nationalityCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Nationality> nationalityQuery, IRepositoryQuery<InvEmployees> invEmployeesQuery)
        {
            _NationalityCommand = nationalityCommand;
            _NationalityQuery = nationalityQuery;
            _InvEmployeesQuery = invEmployeesQuery;
        }

        public async Task<ResponseResult> Handle(DeleteNationalityRequest request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Split(',').Select(c=> int.Parse(c)).ToArray();
            if(!ids.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.DataRequired,
                        MessageEn = ErrorMessagesEn.DataRequired,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };

            //var idsUsed = _InvEmployeesQuery
            //    .TableNoTracking
            //    .Where(c => c.nationalityId != null)
            //    .GroupBy(c => c.nationalityId)
            //    .Select(c => c.FirstOrDefault())
            //    .Select(c=> c.nationalityId).ToArray();

            //ids = ids.Where(c => !idsUsed.Contains(c)).ToArray();
            //if (!ids.Any())
            //    return new ResponseResult
            //    {
            //        Result = Result.Failed,
            //        Alart = new Alart
            //        {
            //            AlartType = AlartType.error,
            //            type = AlartShow.note,
            //            MessageAr = "لا يمكن حذف البيانات المطلوبة",
            //            MessageEn = "Cant delete this data",
            //            titleAr = "خطأ",
            //            titleEn = "Error"
            //        }
            //    };
            var Elements = _NationalityQuery
                .TableNoTracking
                .Include(c=> c.InvEmployees)
                .Where(c => ids.Contains(c.Id))
                .Where(c=> !c.InvEmployees.Any());
            if(!Elements.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = ErrorMessagesAr.CanNotDelete,
                        MessageEn = ErrorMessagesEn.CanNotDelete,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            _NationalityCommand.RemoveRange(Elements);
            var deleted = await _NationalityCommand.SaveAsync();
            if(deleted)
                return new ResponseResult
                {
                    Result = Result.Success,
                    Alart = new Alart
                    {
                        AlartType = AlartType.success,
                        type = AlartShow.note,
                        MessageAr = ErrorMessagesAr.DeletedSuccessfully,
                        MessageEn = ErrorMessagesEn.DeletedSuccessfully,
                        titleAr = "save",
                        titleEn = "save"
                    }
                };
            else
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.note,
                        MessageAr = ErrorMessagesAr.CanNotDelete,
                        MessageEn = ErrorMessagesEn.CanNotDelete,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
        }
    }
}
