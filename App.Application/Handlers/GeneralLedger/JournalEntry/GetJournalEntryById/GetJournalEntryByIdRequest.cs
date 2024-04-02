using App.Application.Basic_Process;
using Attendleave.Erp.Core.APIUtilities;
using MediatR;

namespace App.Application.Handlers.GeneralLedger.JournalEntry
{
    public class GetJournalEntryByIdRequest : IRequest<IRepositoryActionResult>
    {
        public int Id { get; set; }
    }
}
