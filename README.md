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


```

### 3. Check Balance

GET /api/balance/{accountId}

→ Balance computed live from LedgerEntries

→ Never from a stored number that can lie

## Invariant Enforcement

| Rule | Enforcement |
|------|-------------|
| Debits must equal credits | Domain entity throws if sum ≠ 0 |
| No zero or negative amounts | Guard clause in LedgerEntry.Create() |
| Minimum 2 entries per journal | Guard clause in JournalEntry.Create() |
| Currency must match account | Validated in JournalEntryService |
| No duplicate requests | Unique index on IdempotencyKey |


## Architecture Diagram

<img width="5553" height="1956" alt="mermaid-diagram (6)" src="https://github.com/user-attachments/assets/5a9dc270-7d62-4803-9710-06369ced4a5a" />


## How to Run

### With Docker (recommended):

```bash
git clone https://github.com/Sarthak12397/Financial_ledger_system.git
cd Financial_ledger_system
docker-compose up --build
```

API runs on http://localhost:8080

### Without Docker:

```bash
git clone https://github.com/Sarthak12397/Financial_ledger_system.git
cd Financial_ledger_system
```

Configure `appsettings.json` with your PostgreSQL connection string.

```bash
dotnet restore
dotnet build
dotnet ef database update
dotnet run
```

API runs on http://localhost:5025

Hangfire dashboard: http://localhost:5025/hangfire


## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/accounts` | Create account |
| GET | `/api/accounts` | Get all accounts |
| GET | `/api/accounts/{id}` | Get account by ID |
| DELETE | `/api/accounts/{id}` | Deactivate account |
| POST | `/api/ledger` | Post a journal entry |
| GET | `/api/ledger/{id}` | Get journal entry by ID |
| GET | `/api/ledger/account/{accountId}` | Get entries for account |
| GET | `/api/balance/{accountId}` | Get computed balance |


## Design Decisions

| Decision | Why |
|----------|-----|
| Double-entry bookkeeping | 800 year old rule — every debit has a credit |
| Immutable ledger entries | No UPDATE ever — history cannot be rewritten |
| Idempotency keys | Same request, same result — no duplicate money movement |
| Balance computed from entries | Stored balance can lie — ledger entries cannot |
| PostingStatus state machine | System always knows if a journal succeeded or failed |
| Atomic transaction boundary | All or nothing — partial writes are impossible |
| Correlation IDs | Full traceability across requests and retries |
| Hangfire | Scheduled balance verification survives restarts |


## What I'd Improve

| Improvement | Reason | Impact | Priority |
|-------------|--------|--------|----------|
| Composite idempotency key (ClientId + Key) | Prevent cross-client key collisions | High | High |
| Reversal journal entries | Correct mistakes without mutating history | High | High |
| JWT Authentication | Secure endpoints — only authenticated users post entries | High | High |
| Pagination on ledger queries | GetByAccountIdAsync unusable at scale | Medium | Medium |
| OpenTelemetry | Distributed tracing beyond single-service correlation IDs | Medium | Medium |
| Rate limiting | Prevent API abuse | High | High |


