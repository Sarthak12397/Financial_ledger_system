public interface ILedgerRepository
{
    Task<JournalEntry?>GetByIdAsync   (Guid id, CancellationToken ct = default);
     Task<IEnumerable<LedgerEntry>> GetByAccountIdAsync(Guid id, CancellationToken ct = default);
     

}