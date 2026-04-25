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
    public AccountType AccountTypes
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

 public Account(Guid id, string name, AccountType accountTypes, string currency, decimal balance, DateTime createdAt, bool isactive)
{
    Id = id;
    Name = name;
    AccountTypes = accountTypes;
    Currency = currency;
    Balance = balance;
    CreatedAt = createdAt;
    IsActive = isactive;
}
}