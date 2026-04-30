using Microsoft.EntityFrameworkCore;

public class JournalEntryService
{
     private readonly ILedgerRepository _ledger;
    private readonly IAccountRepository _accounts;
    private readonly IUnitOfWork _uow;
private readonly LedgerDbContext _context; 

    public JournalEntryService(  ILedgerRepository ledger,  IAccountRepository accounts,  IUnitOfWork uow, LedgerDbContext context)
    {
        _ledger = ledger;
        _accounts = accounts;
        _uow = uow;
         _context = context; 
    }


    public async Task<JournalEntry>PostAsync(string descripition, string idempotencykey, List<LedgerEntryRequest> requests, CancellationToken ct = default)
    {
        
var existing = await _ledger.GetByIdempotencyKeyAsync(idempotencykey, ct);
if (existing != null) return existing;

foreach (var request in requests)
{
    var exists = await _accounts.ExistsAsync(request.AccountId, ct);
    if (!exists) throw new Exception($"Account {request.AccountId} does not exist.");
}

foreach (var request in requests)
{
    var account = await _accounts.GetByIdAsync(request.AccountId, ct);
    if (account!.Currency != request.currency)
        throw new Exception($"Currency mismatch on account {request.AccountId}.");
}

var entries = requests.Select(r =>
    LedgerEntry.Create(r.AccountId, Guid.Empty, r.Amount, r.Description, r.EntryType))
    .ToList();

var journal = JournalEntry.Create(descripition, idempotencykey, entries);

foreach (var request in requests)
{
    var account = await _accounts.GetByIdAsync(request.AccountId, ct);
    if (request.EntryType == EntryType.Debit)
        account!.Debit(request.Amount);
    else
        account!.Credit(request.Amount);
}
using var tx = await _context.Database.BeginTransactionAsync(ct);

try
{
    await _ledger.CreateJournalEntryAsync(journal, ct);
    journal.MarkPosted();
    await _uow.SaveChangesAsync(ct);
    await tx.CommitAsync(ct);
    return journal;
}
catch (DbUpdateConcurrencyException)
{
    await tx.RollbackAsync(ct);
    throw new InvalidOperationException(
        "Concurrency conflict detected. Please retry.");
}
catch
{
    await tx.RollbackAsync(ct);
    throw;
}

    }
}
