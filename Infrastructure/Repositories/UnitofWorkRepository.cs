public class UnitofWorkRepository: IUnitOfWork
{
      private readonly LedgerDbContext _context;


    public UnitofWorkRepository(LedgerDbContext context)
    {
        _context = context;
    }
    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return await _context.SaveChangesAsync(ct);
    }



}