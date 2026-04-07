# Product Management System (RESTful API)

This is a complete RESTful API solution for managing products, built with **.NET 8** and following **Clean Architecture** principles.

## 🚀 Features
- **Full CRUD Operations**: Create, Read, Update, and Delete products.
- **Security**: JWT Authentication with Refresh Token strategy.
- **Validation**: Data validation using **FluentValidation**.
- **Logging**: Structured logging with **Serilog** (Console & File).
- **Documentation**: Fully documented APIs with **Swagger/OpenAPI**.

## 🛠️ Tech Stack
- **Framework**: .NET 8
- **Database**: SQL Server with Entity Framework Core
- **Testing**: xUnit & Moq
- **Containerization**: Docker & Docker Compose

## 🏗️ Architecture
The project follows **Clean Architecture**:
- **Domain**: Entities and Business Logic.
- **Application**: DTOs, Services, and Interfaces.
- **Infrastructure**: Data access (Generic Repository, Unit of Work).
- **API**: Controllers and Middleware.

## 🚦 How to Run
1. Clone the repository.
2. Update the connection string in `appsettings.json`.
3. Run `Update-Database` in Package Manager Console.
4. (Optional) Run using Docker: `docker-compose up`

## 🧪 Testing
Run the unit tests via Test Explorer in Visual Studio to see the green checks ✅.
