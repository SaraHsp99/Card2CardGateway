# 💳 Card2Card Gateway

A sample **Card-to-Card Transfer Gateway** built with **.NET 7**, demonstrating clean architecture, design patterns, and best practices for back-end development.  

This project was developed as part of a **Back-End Developer** coding assignment.  

---

## 🚀 Features

- **Bank Service Factory Pattern** → dynamically resolves the correct bank service (e.g., Saman, Pasargad) based on the source card BIN.  
- **Repository & Unit of Work Pattern** → provides clean data access and transaction management.  
- **In-Memory Database (EF Core)** → lightweight, no migration required.  
- **Validation (FluentValidation)** → validates transfer requests (amount, card number, expiration date, etc.).  
- **Transaction Management** → supports transfer, inquiry, and history retrieval.  
- **Middlewares**:
  - Global Exception Handling
  - Request/Response Logging  
- **Unit Tests (xUnit + Moq)** → covers core scenarios (duplicate transaction, successful transfer, inquiry, etc.).  

---

## 🛠️ Tech Stack

- **.NET 7**
- **Entity Framework Core (In-Memory provider)**
- **FluentValidation**
- **Polly (resiliency & retries)**
- **xUnit & Moq**
- **Serilog / ILogger**

---

## 📂 Project Structure

Card2CardGateway
├── Application
│ ├── UseCases
│ │ ├── Transfer
│ │ │ ├── DTOs
│ │ │ ├── Interfaces
│ │ │ ├── Options
│ │ │ ├── Services
│ │ │ └── Validators
│ ├── Common
│ └── Options
├── Domain
│ ├── Entities
│ └── Enums
├── Infrastructure
│ ├──Middleware
│ ├── Persistence (DbContext, Repositories, UnitOfWork)
├── WebApi
│ ├── Controllers
└── Tests
└── UnitTests
---

## ⚡ Getting Started

### 1️⃣ Clone the Repository
```bash
git clone https://github.com/SaraHsp99/Card2CardGateway.git
cd Card2CardGateway
2️⃣ Run the Application
dotnet run --project src/WebApi

3️⃣ Test the APIs (Swagger UI enabled)

Transfer: POST /api/transactions/transfer

Inquiry: GET /api/transactions/inquiry/{requestTraceId}

History: GET /api/transactions/history

4️⃣ Run Tests
dotnet test

🔑 Example Transfer Request
{
  "requestTraceId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "amount": 50000,
  "sourceCardNumber": "6219861234567890",
  "destinationCardNumber": "5022291234567890",
  "cvv2": "123",
  "pin2": "1234",
  "expiredDate": "2608"
}

🧪 Unit Tests

✅ Duplicate transaction detection
✅ Successful transfer execution
✅ Inquiry updates transaction status
✅ History retrieval by status/date

👩‍💻 Author

Sara Hosseinpanahi
GitHub Profile

📌 Notes

The project uses EF Core InMemory Database, so no migrations are required.

This project is designed for demonstration purposes and does not connect to real bank services.

