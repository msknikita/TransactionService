# TransactionService

API сервис для создания, хранения и получения транзакций с учетом требований идемпотентности и валидации

# API Endpoints
## POST /api/v1/Transaction
```json
{
  "id": "1afa615f-af61-4d8a-b891-bc874c937772",
  "transactionDate": "2024-10-25T00:00:00+05:00",
  "amount": 12.34
}
```

## Response 200 OK:
```json
{
  "insertDateTime": "2024-10-25T12:03:34+05:00"
}
```

## GET /api/v1/Transaction?id=1afa615f-af61-4d8a-b891-bc874c937772
## Response 200 OK:
```json
{
  "id": "1afa615f-af61-4d8a-b891-bc874c937772",
  "transactionDate": "2024-10-25T00:00:00+05:00",
  "amount": 12.34
}
```
