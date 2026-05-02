using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IAccountRepository _accounts;
    private readonly IUnitOfWork _uow;

    public AccountsController(IAccountRepository accounts, IUnitOfWork uow)
    {
        _accounts = accounts;
        _uow = uow;
    }


    [HttpGet("{id}")]
  public async Task<ActionResult> GetById(Guid id)
{
   var account = await _accounts.GetByIdAsync(id);
   if(account == null)
        {
            return NotFound();

        }
    return Ok(account);
 
}

[HttpDelete("{id}")]
public async Task<ActionResult> Deactivate(Guid id)
{

   var account = await _accounts.GetByIdAsync(id);

   if(account== null)
        {
             return NotFound();
        }
account.Deactivate();
            await _uow.SaveChangesAsync();
    return NoContent();
}



[HttpGet]
public async Task<ActionResult> GetAllAsync()
{
    var accounts = await _accounts.GetAllAsync();

    return Ok(accounts);
}

[HttpPost] 
public async Task<ActionResult> Create([FromBody] CreateAccountRequest request)


{
        var account = Account.Create(request.Name, request.AccountType, request.Currency);
    await _accounts.CreateAsync(account);
    await _uow.SaveChangesAsync();
    return CreatedAtAction(nameof(GetById), new { id = account.Id }, account);
}


   }