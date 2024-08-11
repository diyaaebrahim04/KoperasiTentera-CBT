# KoperasiTentera-CBT

## Overview
This project is a backend API developed using .NET 8.0 ASP.NET and Entity Framework Core. It is designed to handle a mobile application's user management and verification processes.

<img width="1226" alt="image" src="https://github.com/user-attachments/assets/8da9e78b-f127-4278-b12b-8627b76f3ca2">


## Features
- User Registration
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
- `POST /api/user/login-initiation` - User login initiation
- `POST /api/user/login-completion` - User login completion
- `POST /api/user/verify-otp` - Verify OTP for Email/Mobile
- `POST /api/user/create-pin` - Create user PIN

## Notes
- No third-party integrations were included as per assessment instructions.
- The focus is on the core logic and API flow.
