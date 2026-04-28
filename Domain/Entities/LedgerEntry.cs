public class LedgerEntry
{
    public Guid Id{get; private set;}

    public Guid JournalEntryId{get; private set;}

    public Guid AccountId{get; private set;}
    public EntryType Entrytype{get; private set;}
    public decimal Amount{get; private set;}
    public string Description{get; private set;}
    public DateTime CreatedAt{get; private set;}


    private LedgerEntry()
    {
        
    }

    public static LedgerEntry Create(Guid accountid, Guid journalentryid, decimal amount, string description, EntryType entrytypes  )

    {
            if (amount <= 0)
        throw new ArgumentException("Amount must be positive.");
     return new LedgerEntry
     {
        
         Id = Guid.NewGuid(),
         AccountId = accountid,
         JournalEntryId = journalentryid,
         Entrytype = entrytypes,
         Amount = amount,
         Description = description,
         CreatedAt = DateTime.UtcNow

     }   ;
    }



}