using App.Infrastructure;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Application.Handlers.AttendLeaving.Projects.AddProject
{
    public class AddProjectHandler : IRequestHandler<AddProjectRequest, ResponseResult>
    {
        private readonly IRepositoryCommand<App.Domain.Entities.Process.AttendLeaving.Projects> _ProjectsCommand;

        public AddProjectHandler(IRepositoryCommand<Domain.Entities.Process.AttendLeaving.Projects> projectsCommand)
        {
            _ProjectsCommand = projectsCommand;
        }

        public async Task<ResponseResult> Handle(AddProjectRequest request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.arabicName.Trim()))
                return new ResponseResult
                {
                    Result = Result.Failed,
                    Alart = new Alart
                    {
                        AlartType = AlartType.error,
                        type = AlartShow.popup,
                        MessageAr = ErrorMessagesAr.arabicNameIsRequired,
                        MessageEn = ErrorMessagesEn.arabicNameIsRequired,
                        titleAr = "خطأ",
                        titleEn = "Error"
                    }
                };
            if (string.IsNullOrEmpty(request.latinName.Trim()))
                request.latinName = request.arabicName.Trim();
            var saved = await _ProjectsCommand.AddAsync(new Domain.Entities.Process.AttendLeaving.Projects
            {
                arabicName = request.arabicName.Trim(),
                latinName = request.latinName.Trim()
            });
            return new ResponseResult
            {
                Result = saved ? Result.Success : Result.Failed,
                Alart = saved ? new Alart { AlartType = AlartType.success, type = AlartShow.note, MessageAr = ErrorMessagesAr.SaveSuccessfully, MessageEn = ErrorMessagesEn.SaveSuccessfully, titleAr = "save", titleEn = "save" } : new Alart { AlartType = AlartType.error, type = AlartShow.popup, MessageAr = ErrorMessagesAr.ErrorSaving, MessageEn = ErrorMessagesEn.ErrorSaving, titleAr = "خطأ", titleEn = "Error" }
            };
        }
    }
}
