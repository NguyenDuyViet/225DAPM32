# Backend RESTful API

## Cấu trúc dự án

```
Backend/
├── Controllers/         # API Controllers
├── Models/             # Entity Models (Database models)
├── Services/           # Business Logic Services
├── Repositories/       # Data Access Layer
├── DTOs/               # Data Transfer Objects
├── Middlewares/        # Custom Middlewares
├── Utilities/          # Helper/Utility Classes
├── Program.cs          # Main application configuration
├── appsettings.json    # Application settings
└── Backend.csproj      # Project file
```

## Yêu cầu

- .NET 9.0 SDK trở lên
- SQL Server hoặc Database khác

## Chạy dự án

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run

# Run in watch mode (auto-reload)
dotnet watch run
```

## API Endpoints

Các API endpoints sẽ được định nghĩa trong thư mục Controllers

- GET /api/users - Lấy danh sách users
- GET /api/users/{id} - Lấy user theo ID
- POST /api/users - Tạo user mới
- PUT /api/users/{id} - Cập nhật user
- DELETE /api/users/{id} - Xóa user

## Port mặc định

API sẽ chạy trên: `https://localhost:5001` hoặc `http://localhost:5000`
