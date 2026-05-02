public class BalanceService
{
    
     private readonly ILedgerRepository _ledger;
    private readonly IAccountRepository _accounts;
        public BalanceService(ILedgerRepository ledger, IAccountRepository accounts)
    {
        _ledger = ledger;
        _accounts = accounts;
    }


        public async Task<decimal> GetBalanceAsync(Guid accountId, CancellationToken ct = default )
    {
        var entries = await _ledger.GetByAccountIdAsync(accountId, ct);

      decimal balance = 0m;
        foreach (var entry in entries)
        {
    if (entry.Entrytype == EntryType.Credit)
        balance += entry.Amount;
    else
        balance -= entry.Amount;
        }
    return balance; 
    }
        public async Task VerifyAllBalancesAsync(CancellationToken ct = default)
    {
     var accounts = await _accounts.GetAllAsync(ct);
     foreach(var account in accounts)
        {
            var computed = await GetBalanceAsync(account.Id, ct);
            if(account.Balance != computed)
            {
        Console.WriteLine($"MISMATCH: Account {account.Id} stored={account.Balance} computed={computed}");
            }
        }

    }

}