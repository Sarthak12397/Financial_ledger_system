public record PostJournalEntryDto(
    string Description,
    string IdempotencyKey,
    List<LedgerEntryRequest> Entries  
);