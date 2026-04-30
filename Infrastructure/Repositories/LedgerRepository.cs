using Microsoft.EntityFrameworkCore;

public class LedgerRepository: ILedgerRepository
{
    private readonly LedgerDbContext _context;


    public LedgerRepository(LedgerDbContext context)
    {
        _context = context;
    }


    public async Task<JournalEntry?> GetByIdAsync(Guid id,  CancellationToken ct = default)
    {
            return await _context.JournalEntries  .Include(x => x.Entries) .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
     public async Task<IEnumerable<LedgerEntry>> GetByAccountIdAsync(Guid id, CancellationToken ct = default)
            {
                      return await _context.JournalEntries
                              .SelectMany(j => j.Entries)
                              .Where(e => e.AccountId == id)
                .ToListAsync(ct);
            }

        public async Task CreateJournalEntryAsync (JournalEntry entry, CancellationToken ct = default)
    {
            await _context.JournalEntries.AddAsync(entry, ct);

    }

            public async Task <JournalEntry?> GetByIdempotencyKeyAsync(string key, CancellationToken ct = default){
                return await _context.JournalEntries.Include(x => x.Entries).FirstOrDefaultAsync(x => x.IdempotencyKey  == key, ct);
            }






}