# Talabat.Solution 🍔  
An ASP.NET Core Web API for a food delivery e-commerce system.

---

## 📚 Overview

This project simulates the backend system of a Talabat-like platform. It allows users to register, browse products, manage baskets, place orders, and handle payments — all through a clean RESTful API.

---

## 🚀 Tech Stack

- **ASP.NET Core Web API**
- **Entity Framework Core**
- **SQL Server**
- **AutoMapper**
- **JWT Authentication**
- **Swagger UI**
- **Clean Architecture (API/Core/Repository/Service)**

---

## 🧱 Project Structure

Talabat.Solution/
├── Talabat.APIs         → API Layer (Controllers, Middleware)
├── Talabat.Core         → Entities, Interfaces, Specifications
├── Talabat.Repository   → Data Access Layer (EF + Configs)
├── Talabat.Service      → Business Logic


---

## 🔗 API Endpoints

### 🔐 Account
| Method | Endpoint                    | Description                          |
|--------|-----------------------------|--------------------------------------|
| POST   | `/api/Account/login`        | User login and returns JWT token     |
| POST   | `/api/Account/register`     | Register a new user                  |
| GET    | `/api/Account`              | Get current authenticated user       |
| GET    | `/api/Account/address`      | Get user address                     |
| PUT    | `/api/Account/address`      | Update user address                  |
| GET    | `/api/Account/emailExist`   | Check if email already exists        |

### 🛒 Basket
| Method | Endpoint         | Description                         |
|--------|------------------|-------------------------------------|
| GET    | `/api/Basket`    | Get user's current basket           |
| POST   | `/api/Basket`    | Update/Add basket items             |
| DELETE | `/api/Basket`    | Clear the basket                    |

### 📦 Orders
| Method | Endpoint                      | Description                        |
|--------|-------------------------------|------------------------------------|
| POST   | `/api/Orders`                 | Create new order                   |
| GET    | `/api/Orders`                 | Get user’s order history           |
| GET    | `/api/Orders/{id}`            | Get order details by ID            |
| GET    | `/api/Orders/deliveryMethod`  | Get available delivery methods     |

### 💳 Payment
| Method | Endpoint                    | Description                              |
|--------|-----------------------------|------------------------------------------|
| POST   | `/api/Payment/{basketId}`   | Create/update Stripe payment intent      |
---

## 🛡️ Authentication

- Token-based (JWT)
- Use Bearer token in headers:

## 🛠️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/abdoyasser15/Talabat.Solution.git
cd Talabat.Solution

Edit
appsettings.json
and update:

"ConnectionStrings": {
  "DefaultConnection": "your-sql-connection"
},
"Jwt": {
  "Key": "your-jwt-secret-key"
}
        
dotnet ef database update
dotnet run
