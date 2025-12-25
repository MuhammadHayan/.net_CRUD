# .netCrud
A **professional, secure, and scalable RESTful API** built with **ASP.NET Core 9** and **MongoDB**.  
This project demonstrates a clean implementation of **JWT Authentication**, the **Service-Repository Pattern**, and **DTO-based data handling**.

---

## 🛠 Tech Stack

- **Framework:** .NET 9 (ASP.NET Core)  
- **Database:** MongoDB  
- **Authentication:** JWT (JSON Web Tokens)  
- **Security:** BCrypt.Net for password hashing  
- **API Documentation:** Scalar (Modern Swagger Alternative) & OpenAPI  
- **Architecture:** N-Tier / Layered Architecture  

---

## 🏗 Project Architecture

This project follows a **Layered Architecture** to ensure separation of concerns and maintainability:

- **Controllers:** Handle HTTP requests and manage API routing.  
- **Services:** Contain business logic and interact with the MongoDB driver.  
- **Models:** Represent the database schema (Data Entities).  
- **DTOs (Data Transfer Objects):** Define the contract between the client and server, ensuring sensitive data (like password hashes) is never exposed.  
- **Middleware:** Centralized error handling and security processing.  

---

## 🔑 Key Features

- **Secure Authentication:** Complete Login and Registration flow with hashed passwords.  
- **Standardized Responses:** Every API request returns a consistent JSON envelope (`Success`, `Message`, `Data`).  
- **Global Error Handling:** Custom middleware to catch exceptions and return user-friendly errors.  
- **Input Validation:** Strict validation using Data Annotations (Email formats, string lengths, etc.).  
- **Scalable DB Access:** Singleton database service for efficient connection pooling.  

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)  
- [MongoDB Community Server](https://www.mongodb.com/try/download/community) (running locally on port 27017)  

## Installation & Run

## Clone the repository
git clone https://github.com/yourusername/MongoCrudApi.git

## Navigate to the project folder
cd .netCrud

## Restore dependencies
dotnet restore

## Run the application
dotnet run
The API will be available at: http://localhost:5160

📍 API Endpoints
- Auth (Public)
- Method	Endpoint	Description
- POST	/api/auth/register	Register a new user
- POST	/api/auth/login	Login and receive a JWT Token

Users (Protected - Requires JWT)
- Method	Endpoint	Description
- GET	/api/users	Get all users
- GET	/api/users/{id}	Get a specific user by ID
- POST	/api/users	Create a new user
- PUT	/api/users/{id}	Update an existing user
- DELETE	/api/users/{id}	Delete a user

📖 API Documentation
Once the app is running, explore the interactive API documentation using Scalar:

🔗 http://localhost:5160/scalar/v1

### Configuration
Update the `appsettings.json` file with your credentials:

```json
{
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_KEY_HERE",
    "Issuer": "MongoCrudApi",
    "Audience": "MongoCrudApiUsers"
  }
}
