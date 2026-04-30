public class Account
{
    public Guid Id
    {
        get; private set;
    }
    public string Name
    {
        get; private set;
    }
    public AccountType AccountType
    {
        get; private set;
    }
    public string Currency
    {
        get; private set;
    }
    public decimal Balance
    {
        get; private set;
        
    }
    public DateTime CreatedAt
    {
        get; private set;
    }
    public bool IsActive
    {
        get; private set;
    }
    public byte[] RowVersion { get; private set; }


private Account()
    {
        
    }
    public static Account Create(string name, AccountType accounttype, string currency)
    {
        return new Account{
         Id = Guid.NewGuid(),
         Name = name,
         AccountType = accounttype,
         Currency = currency,
         Balance = 0m,
         CreatedAt = DateTime.UtcNow,
         IsActive = true
        };
    }
        public void Deactivate() => IsActive = false;

        public void Debit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new Exception("Debit not fullfilled");
        }
        Balance -= amount;
    }

    public void Credit(decimal amount)
    {
        if(amount <= 0)
        {
            throw new Exception("Balance");
        }
        Balance += amount;
    }

}
