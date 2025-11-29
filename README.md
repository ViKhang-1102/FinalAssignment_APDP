# FinalAssignment-APDP

Ứng dụng quản lý sinh viên và khóa học được xây dựng bằng Blazor Server targeting .NET 8. Ứng dụng sử dụng ASP.NET Core Identity và Entity Framework Core để quản lý người dùng, vai trò, khoa, chuyên ngành, môn học và khóa học.

## Mục lục
- Yêu cầu hệ thống
- Cài đặt & Thiết lập
- Chạy dự án
- Kiểm tra
- Chức năng chính
- Cấu trúc dự án
- Đóng góp

## Yêu cầu hệ thống
- .NET 8 SDK
- SQL Server hoặc SQL Server Express
- Visual Studio 2022 (hoặc VS Code + C# extension)
- Git

## Cài đặt & Thiết lập

1. Clone repository
```bash
git clone https://github.com/tmthanhCT/FinalAssignment-APDP.git
cd FinalAssignemnt-APDP
```

2. Cấu hình chuỗi kết nối
- Mở file `appsettings.json` và sửa `ConnectionStrings:DefaultConnection` cho phù hợp với môi trường của bạn. Ví dụ:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=SIMSDB;Trusted_Connection=True;Encrypt=false"
  }
}
```
- Nếu muốn, tạo database bằng SSMS hoặc:
```sql
CREATE DATABASE SIMSDB;
```

3. Khôi phục package và áp dụng migrations
```bash
dotnet restore
dotnet ef database update
```

4. Thiết lập Identity
- Khi chạy ứng dụng lần đầu, EF Core sẽ tạo các bảng Identity nếu chưa tồn tại.

## Chạy dự án

- Visual Studio: mở `FinalAssignemnt_APDP.sln` hoặc `FinalAssignemnt_APDP.csproj`, chọn profile và chạy (F5).
- Dòng lệnh:
```bash
dotnet run --project FinalAssignemnt_APDP.csproj
```
- Để phát triển nhanh: `dotnet watch run` (nếu cần).

## Kiểm tra

- Mở trình duyệt tới `https://localhost:{port}` (port hiển thị khi chạy).
- Tạo tài khoản mới hoặc đăng nhập.
- Truy cập các trang demo như `Counter`, `Weather` và các trang được bảo vệ (ví dụ `/auth`).

## Chức năng chính

- Xác thực và phân quyền
  - Đăng ký, đăng nhập, đăng xuất
  - Quản lý hồ sơ người dùng
  - Role-based authorization (vai trò)

- Quản lý dữ liệu học thuật
  - `Department` (Khoa)
  - `Major` (Chuyên ngành)
  - `Subject` (Môn học)
  - `Course` (Khóa học)

- Quản lý người dùng
  - Tạo/sửa/xóa người dùng
  - Gán vai trò (Sinh viên/ Giảng viên / Admin)

- Giao diện
  - Blazor Server với layout responsive và sidebar điều hướng
  - Các trang chính: `Home`, `Counter`, `Weather`, trang bảo vệ `Auth`

- Dịch vụ & API
  - `UserService` xử lý logic người dùng và vai trò
  - Sử dụng Entity Framework Core cho tương tác database

## Cấu trúc dự án

```
FinalAssignemnt-APDP/
├── Components/           # Component phân theo chức năng (Account, Layout, Pages)
├── Data/                 # DbContext, ApplicationUser, Services, Models
│   ├── ApplicationDbContext.cs
│   ├── ApplicationUser.cs
│   ├── UserService.cs
│   └── Models/           # Course, Department, Major, Subject, ...
├── wwwroot/              # Static files
├── Program.cs            # Entry point, DI và middleware
├── appsettings.json      # Cấu hình (connection string)
└── FinalAssignemnt_APDP.csproj
```

## Đóng góp

- Fork repository
- Tạo branch cho feature/fix mới
- Commit và tạo Pull Request

## Giấy phép

Dự án được phát hành theo giấy phép MIT.
