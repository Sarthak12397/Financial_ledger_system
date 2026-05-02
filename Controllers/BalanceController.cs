using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/balance")]
public class BalanceController : ControllerBase
{
    private readonly BalanceService _balance;
    private readonly IAccountRepository _accounts;


    public BalanceController(BalanceService balance, IAccountRepository accounts)
    {
        _balance = balance;
        _accounts = accounts;
    }
    [HttpGet("{accountId}")]
      public async Task<ActionResult> GetByBalanceId(Guid accountId, CancellationToken ct)
    {
           var account = await _accounts.GetByIdAsync(accountId, ct);
    if (account == null) return NotFound();
        var balances = await _balance.GetBalanceAsync(accountId);
        return Ok(new AccountBalanceDto(
    accountId,
       account.Name,    
        account.Currency,
    balances,     
    DateTime.UtcNow));
    }

}