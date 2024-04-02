using App.Application.Basic_Process;
using App.Domain.Models.Security.Authentication.Response;
using Attendleave.Erp.Core.APIUtilities;
using MediatR;
using System.Threading;

namespace App.Application.Handlers.GeneralLedger.JournalEntry
{
    public class GetJournalEntryFilesHandler : BusinessBase<GLJournalEntry>, IRequestHandler<GetJournalEntryFilesRequest, IRepositoryActionResult>
    {
        private readonly IRepositoryQuery<GLJournalEntryFiles> journalEntryFilesRepositoryQuery;
        private readonly IRepositoryQuery<GLJournalEntryFilesDraft> journalEntryFilesDraftRepositoryQuery;

        public GetJournalEntryFilesHandler(IRepositoryActionResult repositoryActionResult, IRepositoryQuery<GLJournalEntryFiles> journalEntryFilesRepositoryQuery, IRepositoryQuery<GLJournalEntryFilesDraft> journalEntryFilesDraftRepositoryQuery) : base(repositoryActionResult)
        {
            this.journalEntryFilesRepositoryQuery = journalEntryFilesRepositoryQuery;
            this.journalEntryFilesDraftRepositoryQuery = journalEntryFilesDraftRepositoryQuery;
        }
        public async Task<IRepositoryActionResult> Handle(GetJournalEntryFilesRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.JournalEntryId != 0)
                {
                    var journalentry = journalEntryFilesRepositoryQuery.FindAll(q => q.JournalEntryId == request.JournalEntryId);

                    var list = new List<JournalEntriesFilesDto>();
                    foreach (var item in journalentry)
                    {
                        var journal = new JournalEntriesFilesDto();
                        journal.Id = item.Id;
                        journal.File = item.File;
                        journal.JournalEntryId = item.JournalEntryId;
                        char[] chara = { 'M', 'م', 'ص' };
                        string[] fileName = journal.File.Split(chara);
                        journal.FileName = fileName.Last();
                        list.Add(journal);
                    }
                    return repositoryActionResult.GetRepositoryActionResult(list, RepositoryActionStatus.Ok);

                }
                else
                {
                    var journalentry = journalEntryFilesDraftRepositoryQuery.FindAll(q => q.JournalEntryDraftId == request.JournalEntryDraftId);

                    var list = new List<JournalEntriesFilesDto>();
                    foreach (var item in journalentry)
                    {
                        var journal = new JournalEntriesFilesDto();
                        journal.Id = item.Id;
                        journal.File = item.File;
                        journal.JournalEntryId = item.JournalEntryDraftId;
                        char[] chara = { 'M', 'م', 'ص' };
                        string[] fileName = journal.File.Split(chara);
                        journal.FileName = fileName.Last();
                        list.Add(journal);
                    }
                    return repositoryActionResult.GetRepositoryActionResult(list, RepositoryActionStatus.Ok);

                }
            }
            catch (Exception ex)
            {
                return repositoryActionResult.GetRepositoryActionResult(ex);
            }
        }
    }
}
