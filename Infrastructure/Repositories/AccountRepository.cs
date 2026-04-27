public class AccountRepository: IAccountRepository
{
    private readonly LedgerDbContext _context;


    public AccountRepository(LedgerDbContext context)
    {
        _context = context;
    }
}
