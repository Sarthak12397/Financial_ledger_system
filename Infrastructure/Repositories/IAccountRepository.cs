public interface IAccountRepository
{
    
    Task<Account?> GetByIdAsync   (Guid id, CancellationToken ct = default);

  Task CreateAsync(Account account, CancellationToken ct = default);



Task<IEnumerable<Account>> GetAllAsync(CancellationToken ct = default);

Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);


}