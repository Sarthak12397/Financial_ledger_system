public record LedgerEntryRequest(

    Guid AccountId,
    decimal Amount,
    string Description,
    string currency,
    EntryType EntryType

);