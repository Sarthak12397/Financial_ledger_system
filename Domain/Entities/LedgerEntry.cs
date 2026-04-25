public class LedgerEntry
{
    public Guid Id{get; private set;}

    public Guid JournalEntryId{get; private set;}

    public Guid AccountId{get; private set;}
    public EntryType Entrytype{get; private set;}
    public Decimal Amount{get; private set;}
    public string Description{get; private set;}
    public DateTime CreatedAt{get; private set;}


    private LedgerEntry()
    {
        
    }

    public static LedgerEntry Create(Guid accountid, string description, EntryType entrytypes  )

    {
     return new LedgerEntry
     {
         Id = Guid.NewGuid(),
         AccountId = accountid,
         JournalEntryId = Guid.NewGuid(),
         Entrytype = entrytypes,
         Amount = 0m,
         Description = description,
         CreatedAt = DateTime.UtcNow

     }   ;
    }



}