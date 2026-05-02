using Microsoft.EntityFrameworkCore;

public class AccountRepository: IAccountRepository
{
    private readonly LedgerDbContext _context;


    public AccountRepository(LedgerDbContext context)
    {
        _context = context;
    }

 public async Task CreateAsync(Account account, CancellationToken ct = default)
{
    await _context.Accounts.AddAsync(account, ct);
}

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken  ct = default)
    {
        return await _context.Accounts.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

  public async Task<IEnumerable<Account>> GetAllAsync(CancellationToken ct = default)
{
    return await _context.Accounts.ToListAsync(ct);
}

    public async Task<bool> ExistsAsync (Guid id, CancellationToken  ct = default)
    {
       return await _context.Accounts.AnyAsync(x => x.Id == id, ct);
    }

   
}
