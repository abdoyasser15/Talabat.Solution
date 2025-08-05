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
### 📦 Talabat.Solution (E-Commerce Backend Platform)

-  Developed a fully functional backend Web API for a food delivery e-commerce platform, handling products, baskets, orders, users, and payments.
- Followed **Clean Architecture** with layered separation: API, Core, Repository, and Service to enhance maintainability and scalability.
- Built secure **JWT-based authentication** and **role-based authorization** using ASP.NET Core Identity to manage access for different user types.
- Designed and developed **RESTful APIs** for user authentication, basket operations, order processing, and payment integration.
- Integrated **Stripe API** to manage payment intents and simulate checkout functionality.
- Utilized **Entity Framework Core** and **LINQ** for data access, tracking, and querying.
- Applied **OOP**, **SOLID principles**, and patterns like **Repository** and **Unit of Work** to ensure code cleanliness and separation of concerns.
- Documented and tested APIs using **Swagger (OpenAPI)** for easier collaboration and validation.

**Technologies Used:**  
ASP.NET Core Web API, Entity Framework Core, SQL Server, LINQ, Identity, Stripe API, JWT, Swagger, Repository & Unit of Work Patterns, Clean Architecture.

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
```


### 2. Set up configuration
```bash
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
```
### ✍️ Author

-Abdallah Yasser
