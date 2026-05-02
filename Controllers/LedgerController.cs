using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/ledger")]
public class LedgerController : ControllerBase
{
    private readonly ILedgerRepository _ledger;
    private readonly IUnitOfWork _uow;
private readonly JournalEntryService _journalService;

    public LedgerController(ILedgerRepository ledger, IUnitOfWork uow, JournalEntryService journalService)
    {
        _ledger = ledger;
        _uow = uow;
        _journalService = journalService;
    }

     [HttpGet("{id}")]
  public async Task<ActionResult> GetByIdAsync(Guid id)
{
   var ledger = await _ledger.GetByIdAsync(id);
   if(ledger == null)
        {
            return NotFound();

        }
    return Ok(ledger);
 
}

[HttpGet("account/{accountId}")]
public async Task<ActionResult> GetByAccountIdAsync(Guid accountId)
{
    var ledgers = await _ledger.GetByAccountIdAsync(accountId);

    return Ok(ledgers);
}
[HttpPost]
public async Task<ActionResult> Post([FromBody] PostJournalEntryDto dto, CancellationToken ct)
{
    var journal = await _journalService.PostAsync(
        dto.Description,
        dto.IdempotencyKey,
        dto.Entries,
        ct);
    
    return Created($"/api/ledger/{journal.Id}", journal); 

}


}
