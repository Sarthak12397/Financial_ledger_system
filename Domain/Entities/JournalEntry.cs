public class JournalEntry
{
    
    public Guid Id{get; private set;}
    public string Description{get; private set;}
    public DateTime CreatedAt{get; private set;}
    public IReadOnlyList<LedgerEntry> Entries{get; private set;}

    public string IdempotencyKey{get; private set;}



    private JournalEntry(){}
    public static JournalEntry Create(string description, string idempotencyKey, List<LedgerEntry> entry)
      
    {
                if (entry == null || entry.Count < 2)
    throw new ArgumentException("Journal entry requires at least 2 ledger entries.");
             // Invariant
var sum = entry.Sum(e => e.Entrytype == EntryType.Debit 
    ? e.Amount 
    : -e.Amount);   
      if (sum != 0)
       throw new InvalidOperationException("Ledger invariant violated. Debits must equal credits");
        

        return new JournalEntry

        
        {
            
            Id = Guid.NewGuid(),
            Description = description,
            CreatedAt = DateTime.UtcNow,
            Entries = entry.AsReadOnly(),
            IdempotencyKey = idempotencyKey
        };
        
    }
}