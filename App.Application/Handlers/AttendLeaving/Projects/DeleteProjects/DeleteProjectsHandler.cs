using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Projects.DeleteProjects
{
    public class DeleteProjectsHandler : IRequestHandler<DeleteProjectsRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Projects> _ProjectsCommand;
        private readonly IRepositoryQuery<App.Domain.Entities.Process.AttendLeaving.Projects> _ProjectsQuery;

        public DeleteProjectsHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Projects> projectsCommand, IRepositoryQuery<Domain.Entities.Process.AttendLeaving.Projects> projectsQuery)
        {
            _ProjectsCommand = projectsCommand;
            _ProjectsQuery = projectsQuery;
        }

        public async Task<ResponseResult> Handle(DeleteProjectsRequest request, CancellationToken cancellationToken)
        {
            var ids = request.Ids.Split(',').Select(c => int.Parse(c)).ToArray();
            if (!ids.Any())
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
            var elemetsForDelete = _ProjectsQuery.TableNoTracking.Include(c => c.InvEmployees).Where(c => ids.Contains(c.Id) && !c.InvEmployees.Any());
            if (!elemetsForDelete.Any())
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.CanNotDelete,
                        MessageEn = ErrorMessagesEn.CanNotDelete,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            _ProjectsCommand.RemoveRange(elemetsForDelete);
            var saved = await _ProjectsCommand.SaveChanges() > 0 ? true : false;
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.DeletedSuccessfully, MessageEn = ErrorMessagesEn.DeletedSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.CanNotDelete, MessageEn = ErrorMessagesEn.CanNotDelete, titleAr = "خطأ", titleEn = "Error" }
            };

        }
    }
}
