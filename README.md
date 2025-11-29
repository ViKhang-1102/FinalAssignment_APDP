# FinalAssignment-APDP

Dự án này là một ứng dụng web quản lý sinh viên và khóa học được xây dựng bằng Blazor Server (.NET 8). Nó cung cấp các chức năng quản lý người dùng, vai trò, khoa, chuyên ngành, môn học và khóa học với hệ thống xác thực Identity.

## Mục lục
- [Yêu cầu hệ thống](#yêu-cầu-hệ-thống)
- [Cài đặt và thiết lập](#cài-đặt-và-thiết-lập)
- [Chạy dự án](#chạy-dự-án)
- [Triển khai (Deploy)](#triển-khai-deploy)
- [Chức năng chính](#chức-năng-chính)
- [Cấu trúc dự án](#cấu-trúc-dự-án)
- [Đóng góp](#đóng-góp)

## Yêu cầu hệ thống
- .NET 8 SDK
- SQL Server (hoặc SQL Server Express)
- Visual Studio 2022 hoặc IDE hỗ trợ .NET (như VS Code với C# extension)
- Git

## Cài đặt và thiết lập

### 1. Clone dự án
```bash
git clone https://github.com/tmthanhCT/FinalAssignment-APDP.git
cd FinalAssignment-APDP
```

### 2. Thiết lập cơ sở dữ liệu
- Đảm bảo SQL Server đang chạy trên máy.
- Mở file `appsettings.json` và kiểm tra chuỗi kết nối:
  ```json
  {
    "ConnectionStrings": {
      "DefaultConnection": "Server=DESKTOP-4NP5BL0;Database=SIMSDB;Trusted_Connection=True;Encrypt=false"
    }
  }
  ```
  - Thay đổi `Server` thành tên server SQL của bạn nếu cần.
  - Tạo database `SIMSDB` trong SQL Server Management Studio (SSMS) hoặc bằng lệnh:
    ```sql
    CREATE DATABASE SIMSDB;
    ```

### 3. Khôi phục packages và chạy migrations
```bash
dotnet restore
dotnet ef database update
```

### 4. Thiết lập Identity (nếu cần)
- Ứng dụng sử dụng ASP.NET Core Identity cho xác thực.
- Chạy ứng dụng lần đầu để tạo schema Identity tự động.

### Chạy với Visual Studio
- Mở `FinalAssignemnt_APDP.csproj` trong Visual Studio.
- Nhấn F5 để chạy ở chế độ debug.

### Kiểm tra
- Trang chủ sẽ hiển thị giao diện với sidebar điều hướng.
- Đăng ký tài khoản mới hoặc đăng nhập.
- Truy cập các trang như Counter, Weather, Auth (yêu cầu đăng nhập).

## Chức năng chính

### Quản lý người dùng và vai trò
- Đăng ký, đăng nhập, đăng xuất.
- Quản lý hồ sơ người dùng.
- Phân quyền dựa trên vai trò (Role-based authorization).
- Các trang bảo vệ yêu cầu đăng nhập (ví dụ: `/auth`).

### Quản lý dữ liệu
- **Khoa (Department):** Quản lý các khoa trong trường.
- **Chuyên ngành (Major):** Liên kết với khoa.
- **Môn học (Subject):** Danh sách môn học.
- **Khóa học (Course):** Bao gồm thông tin về môn học, giảng viên, kỳ học.
- **Người dùng (User):** Sinh viên, giảng viên với vai trò tương ứng.

### Giao diện người dùng
- Layout responsive với sidebar điều hướng.
- Các trang: Home, Counter (demo), Weather (demo), Auth (bảo vệ).
- Quản lý tài khoản trong `/Account/Manage`.

### API và dịch vụ
- `UserService`: Quản lý người dùng, vai trò, tạo/sửa/xóa user.
- Entity Framework Core cho truy cập database.
- Blazor Server cho tương tác real-time.

## Cấu trúc dự án
```
FinalAssignemnt-APDP/
├── Components/
│   ├── Account/          # Các component xác thực
│   ├── Layout/           # Layout chính và NavMenu
│   ├── Pages/            # Các trang (Home, Counter, etc.)
│   └── App.razor         # Root component
├── Data/
│   ├── ApplicationDbContext.cs  # DbContext
│   ├── ApplicationUser.cs       # Model User
│   ├── UserService.cs           # Service quản lý user
│   └── Models/                  # Các model (Course, Department, etc.)
├── wwwroot/             # Static files (CSS, JS, images)
├── Program.cs           # Entry point
├── appsettings.json     # Cấu hình
└── FinalAssignemnt_APDP.csproj  # Project file
```

## Đóng góp
- Fork repository.
- Tạo branch cho feature mới.
- Commit và push changes.
- Tạo Pull Request.

## Giấy phép
Dự án này được phát hành dưới giấy phép MIT.<parameter name="filePath">README.md