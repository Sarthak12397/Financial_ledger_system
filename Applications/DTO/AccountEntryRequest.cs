public record AccountBalanceDto
(

    Guid AccountId,
    string Accountname,
    string Currency,
    decimal Balance,
    DateTime ComputedAt

);