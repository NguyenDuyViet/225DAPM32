# Món Ngon Tại Nhà - 225DAPM32

Website đặt món và quản lý nhà hàng xây dựng bằng ASP.NET Core MVC và ASP.NET Core Web API. Hệ thống phục vụ bốn nhóm người dùng: khách hàng, nhà hàng, quản trị viên và shipper; hỗ trợ đặt món theo nhà hàng, giỏ hàng, quản lý thực đơn, thống kê, tải ảnh Cloudinary và chat thời gian thực.

## Mục Lục

- [Tính năng chính](#tính-năng-chính)
- [Công nghệ sử dụng](#công-nghệ-sử-dụng)
- [Kiến trúc dự án](#kiến-trúc-dự-án)
- [Yêu cầu môi trường](#yêu-cầu-môi-trường)
- [Cấu hình ứng dụng](#cấu-hình-ứng-dụng)
- [Cài đặt và chạy dự án](#cài-đặt-và-chạy-dự-án)
- [Database, migration và seed data](#database-migration-và-seed-data)
- [Tài khoản demo](#tài-khoản-demo)
- [Luồng sử dụng](#luồng-sử-dụng)
- [API chính](#api-chính)
- [Phân quyền và bảo mật](#phân-quyền-và-bảo-mật)
- [Ảnh và Cloudinary](#ảnh-và-cloudinary)
- [Kiểm tra dự án](#kiểm-tra-dự-án)
- [Triển khai Render và Vercel](#triển-khai-render-và-vercel)
- [Lưu ý phát triển](#lưu-ý-phát-triển)

## Tính Năng Chính

### Khách hàng

- Đăng ký, đăng nhập và đăng xuất.
- Xem danh sách nhà hàng, tìm kiếm và lọc theo danh mục, khu vực, khoảng giá.
- Chọn nhà hàng để xem thực đơn của chính nhà hàng đó.
- Xem món ăn, thức uống và combo; thêm món còn hàng vào giỏ.
- Cập nhật số lượng món trong giỏ, xóa món và tiến hành đặt hàng.
- Xem và cập nhật hồ sơ cá nhân, địa chỉ giao hàng, lịch sử đơn.

### Nhà hàng

- Dashboard nhà hàng: đơn đang xử lý, doanh thu, món bán chạy và thống kê.
- Quản lý thực đơn: thêm, sửa, xóa món; thiết lập số lượng bán trong ngày.
- Tải ảnh món ăn và ảnh nhà hàng lên Cloudinary.
- Quản lý đơn hàng và cập nhật tiến độ xử lý.
- Xem đánh giá, khuyến mãi, thống kê doanh thu.
- Chat thời gian thực với khách hàng thông qua SignalR.
- Đăng xuất trực tiếp từ menu quản lý.

### Quản trị viên

- Dashboard lấy số liệu thật từ API: tổng nhà hàng, tài khoản, đơn hàng và doanh thu trong ngày.
- Theo dõi đơn hàng gần đây và nhà hàng trong hệ thống.
- Xem danh sách người dùng.
- Khóa hoặc mở khóa tài khoản; tài khoản bị khóa không thể đăng nhập.
- Xem danh sách nhà hàng và đơn hàng.
- Đăng xuất từ giao diện quản trị.

### Shipper

- Backend có API dành cho shipper để xem đơn và cập nhật trạng thái giao hàng.
- Giao diện MVC trong `Frontend/Areas/Shipper` hiện là màn hình demo, còn sử dụng dữ liệu tĩnh và cần nối API để dùng như luồng thật.

## Công Nghệ Sử Dụng

| Thành phần | Công nghệ |
| --- | --- |
| Backend API | ASP.NET Core Web API, .NET 9 |
| Frontend | ASP.NET Core MVC / Razor Views, .NET 9 |
| ORM | Entity Framework Core 9 |
| Database đang sử dụng | MySQL 8, Pomelo.EntityFrameworkCore.MySql |
| Xác thực API | JWT Bearer Token |
| Trạng thái đăng nhập frontend | ASP.NET Core Session |
| Hash mật khẩu | BCrypt.Net-Next |
| Lưu trữ ảnh | Cloudinary |
| Chat realtime | ASP.NET Core SignalR |
| API documentation | Swagger / OpenAPI |
| Mapping DTO | AutoMapper |
| UI | Bootstrap, Tailwind utility classes, Chart.js, Font Awesome |

> Lưu ý: project backend còn reference package SQL Server do lịch sử phát triển, nhưng cấu hình chạy hiện tại sử dụng **MySQL** qua `UseMySql(...)`. Không sử dụng file SQL Server cũ để khởi tạo database MySQL.

## Kiến Trúc Dự Án

```text
225DAPM32/
|-- Backend/                         # REST API, business logic, database
|   |-- Controllers/                 # API endpoints
|   |-- Data/SeedData.cs             # Dữ liệu mẫu và ảnh Cloudinary
|   |-- DTOs/                        # Request/response models
|   |-- Hubs/ChatHub.cs              # SignalR chat hub
|   |-- Mapper/                      # AutoMapper profiles
|   |-- Middlewares/Program.cs       # Cấu hình backend startup
|   |-- Migrations/                  # EF Core migrations cho MySQL
|   |-- Models/                      # Entity và AppDbContext
|   |-- Repositories/                # Data access
|   `-- Services/                    # Business logic, upload, auth
|-- Frontend/                        # MVC web application
|   |-- Areas/Admin/                 # Khu vực quản trị viên
|   |-- Areas/Restaurant/            # Khu vực nhà hàng
|   |-- Areas/Shipper/               # Khu vực shipper
|   |-- Controllers/                 # Customer/auth/cart/profile pages
|   |-- Models/                      # View/API models
|   |-- Services/ApiClient.cs        # Gọi Backend API
|   |-- Views/                       # Razor views khách hàng
|   `-- wwwroot/                     # CSS, JS, placeholder images
|-- DB.sql                           # Tệp lịch sử, không dùng cho MySQL hiện tại
`-- README.md
```

### Mô hình hoạt động

```text
Trình duyệt
   |
   v
Frontend MVC (http://localhost:5241)
   |  ApiClient + JWT lưu trong Session
   v
Backend API (http://localhost:8000/api)
   |                         |
   v                         v
MySQL (monngontainha)       Cloudinary / SignalR
```

Frontend không truy cập database trực tiếp. Dữ liệu được lấy và cập nhật thông qua Backend API; token JWT nhận sau đăng nhập được frontend lưu vào session để gọi các endpoint có phân quyền.

## Yêu Cầu Môi Trường

Cần cài đặt:

- .NET SDK 9.0.
- MySQL Server 8.x.
- EF Core command-line tools:

```bash
dotnet tool install --global dotnet-ef
```

- Một tài khoản Cloudinary để tải và seed ảnh.

Kiểm tra phiên bản:

```bash
dotnet --version
dotnet ef --version
```

## Cấu Hình Ứng Dụng

### 1. Backend

Cấu hình trong `Backend/appsettings.json` hoặc dùng User Secrets / biến môi trường khi triển khai thực tế:

```json
{
  "ConnectionStrings": {
    "MySqlConnection": "server=localhost;database=monngontainha;user=YOUR_USER;password=YOUR_PASSWORD;"
  },
  "Jwt": {
    "Key": "YOUR_LONG_RANDOM_JWT_KEY",
    "Issuer": "YOUR_ISSUER",
    "Audience": "YOUR_AUDIENCE",
    "ExpirationMinutes": "120"
  },
  "Cloudinary": {
    "CloudName": "YOUR_CLOUD_NAME",
    "ApiKey": "YOUR_API_KEY",
    "ApiSecret": "YOUR_API_SECRET"
  }
}
```

Không commit password database, JWT key hoặc Cloudinary API secret lên repository công khai. Với môi trường phát triển có thể dùng:

```bash
cd Backend
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:MySqlConnection" "server=localhost;database=monngontainha;user=YOUR_USER;password=YOUR_PASSWORD;"
dotnet user-secrets set "Jwt:Key" "YOUR_LONG_RANDOM_JWT_KEY"
dotnet user-secrets set "Cloudinary:CloudName" "YOUR_CLOUD_NAME"
dotnet user-secrets set "Cloudinary:ApiKey" "YOUR_API_KEY"
dotnet user-secrets set "Cloudinary:ApiSecret" "YOUR_API_SECRET"
```

### 2. Frontend

Frontend gọi API qua khóa cấu hình `Api:BaseUrl`. Nếu không khai báo, ứng dụng dùng địa chỉ phát triển mặc định:

```text
http://localhost:8000/api/
```

Khi chạy bằng biến môi trường ASP.NET Core, dấu `:` được thay bằng `__`:

```bash
Api__BaseUrl=https://YOUR_API_DOMAIN/api/
```

Backend cho phép origin của frontend qua `Cors:AllowedOrigins`, phân tách nhiều URL bằng dấu `;`:

```bash
Cors__AllowedOrigins=https://YOUR_FRONTEND_DOMAIN;https://YOUR_VERCEL_DOMAIN
```

### 3. Cổng mặc định

| Ứng dụng | HTTP | HTTPS |
| --- | --- | --- |
| Backend API | `http://localhost:8000` | `https://localhost:7021` |
| Frontend MVC | `http://localhost:5241` | `https://localhost:7297` |

Swagger backend khi chạy Development:

```text
http://localhost:8000/swagger
```

## Cài Đặt Và Chạy Dự Án

### 1. Restore và build

Từ thư mục gốc:

```bash
dotnet restore Backend/Backend.csproj
dotnet restore Frontend/225DAPM32.csproj

dotnet build Backend/Backend.csproj
dotnet build Frontend/225DAPM32.csproj
```

### 2. Chuẩn bị database

Tạo database MySQL nếu chưa có:

```sql
CREATE DATABASE monngontainha CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
```

Áp dụng migration:

```bash
cd Backend
dotnet ef database update --project Backend.csproj --startup-project Backend.csproj
```

### 3. Tạo dữ liệu mẫu

Seeder tạo role, tài khoản demo, danh mục, 50 nhà hàng, thực đơn có số lượng bán trong ngày, shipper và voucher. Seeder cũng tải ảnh lên Cloudinary dựa trên cấu hình của backend.

```bash
cd Backend
dotnet run --no-build -- --force-seed --seed-only
```

Tham số:

| Tham số | Ý nghĩa |
| --- | --- |
| `--force-seed` | Xóa dữ liệu mẫu hiện tại và tạo lại toàn bộ seed data |
| `--seed-only` | Chạy quá trình khởi tạo/seed rồi thoát, không mở web server |

> `--force-seed` làm mất dữ liệu nghiệp vụ hiện tại trong các bảng seed quản lý. Chỉ dùng cho môi trường demo/phát triển hoặc khi chủ động muốn tạo lại dữ liệu.

### 4. Chạy Backend API

Mở terminal thứ nhất:

```bash
cd Backend
dotnet run --launch-profile http
```

Kiểm tra:

```text
http://localhost:8000/swagger
```

### 5. Chạy Frontend MVC

Mở terminal thứ hai:

```bash
cd Frontend
dotnet run --launch-profile http
```

Truy cập:

```text
http://localhost:5241
```

## Database, Migration Và Seed Data

### Database hiện tại

Ứng dụng dùng `AppDbContext` với MySQL. Các entity chính:

| Nhóm | Entity |
| --- | --- |
| Tài khoản/phân quyền | `User`, `Role`, `Driver` |
| Nhà hàng/thực đơn | `Restaurant`, `Category`, `Food`, `Promotion` |
| Đặt món | `Cart`, `CartFood`, `Order`, `OrderFood`, `OrderPromotion`, `Voucher` |
| Tương tác | `Review`, `ReviewFood`, `ChatMessage`, `Notification`, `Complaint` |
| Khác | `PaymentMethod`, `SystemLog` |

### Migration

Các migration hiện có:

```text
20260512043126_InitialCreate
20260519095527_UpdateDB
20260522123405_Init
20260522144129_AddChatMessages
20260524132708_UpdateDB2
20260525074814_SyncMySqlSchema
```

Migration `SyncMySqlSchema` đồng bộ schema theo MySQL và thêm trường `daily_Quantity` cho món ăn để hỗ trợ trạng thái còn/hết món.

Tạo migration mới sau khi thay đổi model:

```bash
cd Backend
dotnet ef migrations add TenMigration --project Backend.csproj --startup-project Backend.csproj --output-dir Migrations
dotnet ef database update --project Backend.csproj --startup-project Backend.csproj
```

### Seed data hiện tại

Khi chạy seed thành công, dữ liệu demo gồm:

- 4 role: `admin`, `customer`, `restaurant`, `shipper`.
- 7 tài khoản demo.
- 8 danh mục món.
- 50 nhà hàng tại các khu vực Đà Nẵng.
- Khoảng 590+ món; mọi món seed đều có `DailyQuantity > 0` để có thể thử thêm vào giỏ và đặt hàng.
- 2 shipper và dữ liệu voucher mẫu.
- Ảnh nhà hàng, món ăn, danh mục và avatar được lưu trên Cloudinary.

## Tài Khoản Demo

Sau khi chạy lại seed, có thể đăng nhập bằng các tài khoản sau:

| Vai trò | Username | Password | Trang sau đăng nhập |
| --- | --- | --- | --- |
| Admin | `admin` | `123456` | `/admin/Admin/Index` |
| Customer | `customer01` | `123456` | Trang cá nhân/customer |
| Customer | `customer02` | `123456` | Trang cá nhân/customer |
| Customer | `customer03` | `123456` | Trang cá nhân/customer |
| Restaurant | `restaurant01` | `123456` | `/restaurant/Restaurant/Index` |
| Shipper | `shipper01` | `123456` | Không tự redirect; UI `/shipper/Shipper/Index` hiện là demo |
| Shipper | `shipper02` | `123456` | Không tự redirect; UI `/shipper/Shipper/Index` hiện là demo |

Tài khoản do admin khóa sẽ có trạng thái `locked` và không thể đăng nhập cho đến khi được mở khóa.

## Luồng Sử Dụng

### Đặt món với customer

1. Đăng nhập bằng tài khoản customer.
2. Vào mục **Nhà hàng**.
3. Chọn **Xem menu** tại một nhà hàng.
4. Nhấn nút `+` trên món còn hàng để thêm vào giỏ.
5. Vào giỏ hàng, chỉnh số lượng và nhập địa chỉ giao.
6. Xác nhận thanh toán/đặt đơn.
7. Theo dõi đơn tại trang cá nhân.

### Quản lý thực đơn với restaurant

1. Đăng nhập `restaurant01`.
2. Vào **Nhà hàng** trong sidebar.
3. Thêm món mới, chọn danh mục, nhập giá/số lượng bán trong ngày và chọn file ảnh.
4. Ảnh được tải lên Cloudinary, URL được lưu qua API.
5. Theo dõi đơn, thống kê hoặc chat ở các tab quản lý.

Khu vực restaurant chỉ cho phép role `restaurant` truy cập. Role khác hoặc người dùng chưa đăng nhập sẽ được trả về trang `404 Not Found`.

### Khóa tài khoản với admin

1. Đăng nhập `admin`.
2. Vào trang **Người dùng**.
3. Nhấn **Khóa** bên tài khoản cần vô hiệu hóa.
4. Trạng thái chuyển sang **Đã khóa**; tài khoản đó không đăng nhập được.
5. Nhấn **Mở khóa** để cho phép đăng nhập trở lại.

## API Chính

Base URL:

```text
http://localhost:8000/api
```

Các endpoint yêu cầu token sử dụng header:

```http
Authorization: Bearer YOUR_JWT_TOKEN
```

### Authentication và người dùng

| Method | Endpoint | Quyền | Mô tả |
| --- | --- | --- | --- |
| `POST` | `/Auth/login` | Public | Đăng nhập, trả JWT |
| `GET` | `/Users` | Admin | Danh sách tài khoản |
| `GET` | `/Users/{id}` | Đã đăng nhập | Xem tài khoản |
| `POST` | `/Users` | Public | Đăng ký customer |
| `PUT` | `/Users/{id}` | Admin/Customer sở hữu tài khoản | Cập nhật hồ sơ |
| `POST` | `/Users/{id}/toggle-status` | Admin | Khóa/mở khóa tài khoản |

### Nhà hàng, món ăn và danh mục

| Method | Endpoint | Quyền | Mô tả |
| --- | --- | --- | --- |
| `GET` | `/Restaurants` | Public | Phân trang/lọc nhà hàng |
| `GET` | `/Restaurants/all` | Public | Danh sách tất cả nhà hàng |
| `GET` | `/Restaurants/{id}` | Public | Chi tiết nhà hàng |
| `GET` | `/Restaurants/{id}/dashboard` | Public trong API hiện tại | Số liệu dashboard; UI restaurant vẫn chặn role |
| `GET` | `/Restaurants/{id}/analytics` | Public trong API hiện tại | Dữ liệu biểu đồ; UI restaurant vẫn chặn role |
| `PUT` | `/Restaurants/{id}` | Restaurant/Admin | Cập nhật nhà hàng |
| `GET` | `/Food/restaurant/{restaurantId}` | Public | Thực đơn nhà hàng |
| `POST` | `/Food` | Restaurant/Admin | Thêm món |
| `PUT` | `/Food/{id}` | Restaurant/Admin | Sửa món |
| `DELETE` | `/Food/{id}` | Restaurant/Admin | Xóa món |
| `PUT` | `/Food/{id}/daily-quantity` | Restaurant/Admin | Cập nhật số lượng bán |
| `GET` | `/Category` | Public | Danh mục |

### Giỏ hàng và đơn hàng

| Method | Endpoint | Quyền | Mô tả |
| --- | --- | --- | --- |
| `GET` | `/Cart` | Đã đăng nhập | Lấy giỏ hàng |
| `POST` | `/Cart/items` | Đã đăng nhập | Thêm món vào giỏ |
| `PUT` | `/Cart/items/{idCartFood}` | Đã đăng nhập | Cập nhật số lượng |
| `DELETE` | `/Cart/items/{idCartFood}` | Đã đăng nhập | Xóa món |
| `POST` | `/Orders/checkout` | Đã đăng nhập | Đặt hàng từ giỏ |
| `GET` | `/Orders/admin` | Admin | Danh sách đơn hệ thống |
| `GET` | `/Orders/restaurant/{idRestaurant}` | Restaurant/Admin | Đơn của nhà hàng |
| `PUT` | `/Orders/{idOrder}/status` | Restaurant/Admin/Shipper | Cập nhật trạng thái |

### Ảnh, đánh giá và chat

| Method | Endpoint | Quyền | Mô tả |
| --- | --- | --- | --- |
| `POST` | `/Upload/restaurant` | Restaurant/Admin | Upload ảnh nhà hàng |
| `POST` | `/Upload/food` | Restaurant/Admin | Upload ảnh món |
| `GET` | `/Review/food/{idFood}` | Public | Đánh giá món |
| `POST` | `/Review/food` | Đã đăng nhập | Gửi đánh giá |
| `GET` | `/Chat/rooms/{restaurantId}` | Public trong API hiện tại | Danh sách phòng chat |
| SignalR | `/chatHub` | Chưa gắn authorize trong backend | Gửi/nhận tin nhắn realtime |

## Phân Quyền Và Bảo Mật

Role trong hệ thống:

| Role | Phạm vi |
| --- | --- |
| `customer` | Xem nhà hàng, giỏ hàng, đặt món, hồ sơ cá nhân |
| `restaurant` | Khu vực nhà hàng, quản lý món và đơn của nhà hàng |
| `admin` | Dashboard hệ thống, người dùng, nhà hàng, đơn hàng |
| `shipper` | Theo dõi và cập nhật đơn giao |

Cơ chế chính:

- Backend cấp JWT sau khi đăng nhập thành công.
- Frontend lưu token trong session và đính kèm token khi gọi API được bảo vệ.
- Backend dùng `[Authorize]` và `[Authorize(Roles = "...")]`.
- Trang restaurant của frontend kiểm tra session role; chỉ role `restaurant` được truy cập.
- Tài khoản `locked` bị từ chối ngay tại luồng đăng nhập backend.
- Mật khẩu được hash bằng BCrypt, không lưu plaintext.

Các điểm cần siết trước khi triển khai production:

- Gắn authorize phù hợp cho API dashboard/analytics của nhà hàng.
- Bảo vệ API chat và SignalR hub bằng xác thực/phân quyền theo người dùng hoặc nhà hàng.
- Nối giao diện shipper với API thật và thêm kiểm tra quyền tại frontend.

## Ảnh Và Cloudinary

Hệ thống hỗ trợ:

- Upload ảnh nhà hàng.
- Upload ảnh món ăn.
- Seed ảnh demo vào các folder Cloudinary.
- Hiển thị placeholder khi URL ảnh rỗng hoặc ảnh tải thất bại.

Khi thêm hoặc cập nhật món trên trang nhà hàng, dùng input chọn file; frontend gửi multipart form sang endpoint upload, backend tải ảnh lên Cloudinary và lưu URL trả về.

Seed data cần mạng và cấu hình Cloudinary hợp lệ. Khi seed được chạy lại thành công, ảnh demo được lưu dưới các nhóm folder như:

```text
DAPM_32/restaurants
DAPM_32/foods
DAPM_32/categories
DAPM_32/users
```

## Kiểm Tra Dự Án

### Build

```bash
dotnet build Backend/Backend.csproj --no-restore
dotnet build Frontend/225DAPM32.csproj --no-restore
```

### Kiểm tra migration

```bash
cd Backend
dotnet ef migrations list --project Backend.csproj --startup-project Backend.csproj
```

### Checklist demo nhanh

1. Truy cập danh sách nhà hàng và xác nhận ảnh hiển thị bình thường.
2. Mở một nhà hàng và xác nhận món không còn đồng loạt báo `Hết món`.
3. Đăng nhập customer, thêm món vào giỏ và thử checkout.
4. Đăng nhập restaurant, thử thêm món có ảnh file và kiểm tra ảnh Cloudinary.
5. Đăng nhập admin, kiểm tra dashboard hiển thị số liệu từ dữ liệu hiện tại.
6. Khóa một customer, xác nhận không đăng nhập được; mở khóa lại sau khi thử.
7. Kiểm tra nút đăng xuất ở giao diện admin và restaurant.

## Triển Khai Render Và Vercel

Ứng dụng gồm hai server ASP.NET Core: `Backend` là API và `Frontend` là MVC render giao diện. Render chạy được cả hai server bằng Docker. Vercel không chạy trực tiếp Dockerfile ASP.NET Core MVC của dự án này; cấu hình `vercel.json` dùng Vercel làm reverse proxy/domain phía trước frontend đã chạy trên Render.

### 1. File triển khai

| File | Mục đích |
| --- | --- |
| `Backend/Dockerfile` | Build và chạy Backend API bằng .NET 9 runtime image. |
| `Frontend/Dockerfile` | Build và chạy Frontend MVC bằng .NET 9 runtime image. |
| `.dockerignore` | Không đưa build output và `appsettings*.json` có secret vào Docker image. |
| `render.yaml` | Tạo hai Render Web Services: `mon-ngon-api` và `mon-ngon-web`. |
| `vercel.json` | Proxy tất cả request Vercel sang frontend Render. |

### 2. Chạy thử Docker ở local

Build hai image từ thư mục gốc:

```bash
docker build -f Backend/Dockerfile -t mon-ngon-api .
docker build -f Frontend/Dockerfile -t mon-ngon-web .
```

Chạy backend với các giá trị cấu hình thực của môi trường local:

```bash
docker run --rm -p 8000:8080 \
  -e "ConnectionStrings__MySqlConnection=YOUR_MYSQL_CONNECTION_STRING" \
  -e "Jwt__Key=YOUR_LONG_RANDOM_JWT_KEY" \
  -e "Jwt__Issuer=YOUR_ISSUER" \
  -e "Jwt__Audience=YOUR_AUDIENCE" \
  -e "Cloudinary__CloudName=YOUR_CLOUD_NAME" \
  -e "Cloudinary__ApiKey=YOUR_API_KEY" \
  -e "Cloudinary__ApiSecret=YOUR_API_SECRET" \
  -e "Cors__AllowedOrigins=http://localhost:5241" \
  mon-ngon-api
```

Chạy frontend và nối tới API trên máy host:

```bash
docker run --rm -p 5241:8080 \
  -e "Api__BaseUrl=http://host.docker.internal:8000/api/" \
  mon-ngon-web
```

Truy cập `http://localhost:5241` để kiểm tra giao diện.

### 3. Deploy Backend Và Frontend Trên Render

1. Push repository lên GitHub/GitLab.
2. Trong Render, chọn **New > Blueprint** và chọn repository có file `render.yaml`.
3. Render tạo service API `mon-ngon-api` và service MVC `mon-ngon-web`.
4. Điền các environment variables được đánh dấu secret/manual trong Blueprint.

Biến môi trường cho `mon-ngon-api`:

| Key | Giá trị cần nhập |
| --- | --- |
| `ConnectionStrings__MySqlConnection` | Connection string đến MySQL production. |
| `Jwt__Key` | Secret ngẫu nhiên đủ dài, không dùng lại khóa development. |
| `Jwt__Issuer` | Ví dụ `MonNgonApi`. |
| `Jwt__Audience` | Ví dụ `MonNgonWeb`. |
| `Cloudinary__CloudName` | Cloud name Cloudinary. |
| `Cloudinary__ApiKey` | API key Cloudinary. |
| `Cloudinary__ApiSecret` | API secret Cloudinary. |
| `Cors__AllowedOrigins` | `https://mon-ngon-web.onrender.com;https://YOUR_PROJECT.vercel.app` |

Biến môi trường cho `mon-ngon-web`:

| Key | Giá trị cần nhập |
| --- | --- |
| `Api__BaseUrl` | `https://mon-ngon-api.onrender.com/api/` |

Nếu Render cấp URL khác tên service dự kiến, thay URL tương ứng trong `Api__BaseUrl`, `Cors__AllowedOrigins` và `vercel.json`.

Dự án hiện sử dụng MySQL; `render.yaml` không tự tạo database MySQL. Hãy sử dụng MySQL production đã có hoặc dịch vụ MySQL bên ngoài Render và nhập connection string vào backend. Với database đang tồn tại, áp dụng migration trước khi sử dụng dữ liệu production:

```bash
cd Backend
dotnet ef database update --project Backend.csproj --startup-project Backend.csproj
```

Không chạy `--force-seed` trên database production có dữ liệu cần giữ.

### 4. Đưa Domain Qua Vercel

Sau khi `mon-ngon-web` đã chạy thành công trên Render:

1. Import cùng repository vào Vercel và đặt Root Directory là thư mục gốc.
2. Chọn framework preset **Other**; Vercel chỉ đọc `vercel.json`, không build ASP.NET Core.
3. Nếu URL frontend Render không phải `https://mon-ngon-web.onrender.com`, cập nhật `destination` trong `vercel.json`.
4. Deploy Vercel và thêm URL Vercel thực tế vào biến `Cors__AllowedOrigins` của backend Render.

Luồng production:

```text
Browser -> Vercel rewrite -> Frontend MVC on Render -> Backend API on Render -> MySQL / Cloudinary
```

Chat SignalR từ trình duyệt kết nối tới backend Render nên origin của domain Vercel phải có trong `Cors__AllowedOrigins`.

### 5. Bảo mật khi deploy

- Không đưa `Backend/appsettings.json` chứa thông tin thật vào image hoặc repository công khai.
- Nếu key/secret đã từng được push lên remote, hãy rotate MySQL password, JWT key và Cloudinary API secret trước khi public deploy.
- Luôn dùng HTTPS URL cho `Api__BaseUrl` và `Cors__AllowedOrigins` ở production.

## Lưu Ý Phát Triển

- `Backend/Middlewares/Program.cs` là startup file của backend trong cấu trúc hiện tại.
- Startup backend gọi `EnsureCreated()` và chạy seed; migration nên được áp dụng chủ động bằng `dotnet ef database update` trước khi khởi chạy.
- Dữ liệu hệ thống phải được thao tác qua Entity Framework Core/API; không cần chạy `DB.sql` để vận hành MySQL hiện tại.
- Seeder có thao tác xóa rồi tạo lại dữ liệu mẫu khi truyền `--force-seed`; không dùng trên database chứa dữ liệu cần giữ.
- Với production, đưa connection string, JWT key và Cloudinary secret sang environment variables hoặc secret manager.
- Có một số cảnh báo nullability trong code hiện tại khi build; chúng không chặn chạy ứng dụng nhưng nên được dọn dần trước khi triển khai chính thức.

## Thành Viên Và Môn Học

- Mã dự án: `225DAPM32`
- Tên đề tài: **Dịch Vụ Món Ngon Tại Nhà**
- Nguyễn Duy Việt - `22115053122150`
- Nguyễn Nữ Khánh Ngọc - `23115053122124`
- Nguyễn Hồ Minh Quân - `23115053122134`
- Nguyễn Công Minh - `23115053122227`
