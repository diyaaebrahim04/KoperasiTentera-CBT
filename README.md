# KoperasiTentera-CBT

## Overview
This project is a backend API developed using ASP.NET Core and Entity Framework Core. It is designed to handle user management and verification processes for a mobile application.

## Features
- User registration
- Email and mobile OTP verification
- Entity Framework Core for database operations
- FluentValidation for input validation

## Getting Started

### Prerequisites
- .NET SDK 8.0

### Setup Instructions
1. **Clone the repository:**
    ```bash
    git clone https://github.com/diyaaebrahim04/KoperasiTentera-CBT.git
    cd KoperasiTentera-CBT
    ```
2. **Restore dependencies:**
    ```bash
    dotnet restore
    ```
3. **Apply migrations and update the database:**
    ```bash
    dotnet ef database update --project ./KoperasiTentera.Infrastructure --startup-project ./KoperasiTentera.API
    ```
4. **Run the application:**
    ```bash
    dotnet run --project ./KoperasiTentera.API
    ```

## API Endpoints
- `POST /api/user/register` - Register a new user
- `POST /api/user/login` - User login
- `POST /api/user/verify-otp` - Verify OTP for Email/Mobile

## Notes
- No third-party integrations were included as per assessment instructions.
- The focus is on the core logic and API flow.
