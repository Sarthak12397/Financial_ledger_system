# Financial Ledger Engine

A backend API that enforces double-entry bookkeeping at the system level —
every debit has a credit, balances are computed from immutable records,
and money can never appear from nowhere.

> Built with .NET 10 · PostgreSQL · Hangfire · Docker · Serilog

> See also: [Payment Processing System](https://github.com/Sarthak12397/TransactionalBusinessAPI)
> — the write-side that processes the transactions this system records.


## Problem

Without a structured ledger system, financial records become unreliable:

- Money appears or disappears due to unchecked balance mutations
- No audit trail — records can be edited or deleted silently
- Duplicate requests create duplicate transactions
- Balance drift goes undetected until an auditor catches it

## Solution

This system solves those problems through four guarantees:

- **Double-entry bookkeeping** — every debit has a credit. Books always net to zero.
- **Immutability** — ledger entries are never updated or deleted. Append only, forever.
- **Idempotency** — duplicate requests return the same result. Money moves once.
- **Balance verification** — a scheduled job recomputes all balances from source of truth and detects drift.


## Journal Entry Lifecycle

Every financial event moves through defined states:

<img width="4190" height="3143" alt="mermaid-diagram (5)" src="https://github.com/user-attachments/assets/701432f4-3427-470f-a9ee-8416801e377c" />


## Example Flow

### 1. Create Accounts

```json
POST /api/accounts
{
  "name": "Cash Account",
  "currency": "USD",
  "accountType": "Asset"
}
```

### 2. Post a Journal Entry

```json
POST /api/ledger
{
  "description": "Customer payment received",
  "idempotencyKey": "payment-001",
  "entries": [
    {
      "accountId": "...",
      "amount": 500,
      "description": "Cash in",
      "currency": "USD",
      "entryType": "Debit"
    },
    {
      "accountId": "...",
      "amount": 500,
      "description": "Revenue",
      "currency": "USD",
      "entryType": "Credit"
    }
  ]
}
```

```json
{
  "id": "cb90f95b-...",
  "status": "Posted",
  "entries": [...],
  "idempotencyKey": "payment-001"
}
```
