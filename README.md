  # FinalAssignment-APDP

  This is an academic management web application (students, lecturers, courses) built with Blazor Server targeting .NET 8. It uses ASP.NET Core Identity for authentication/authorization and Entity Framework Core for data access.

  Purpose: this README provides an English summary of system requirements, installation steps (for VS Code and Visual Studio), how to run the project, verification steps, main features, and the project structure.

  **Table of Contents**
  - [System Requirements](#system-requirements)
  - [Installation](#installation)
    - [Using Visual Studio Code (IDE)](#using-visual-studio-code-ide)
    - [Using Visual Studio (VS)](#using-visual-studio-vs)
  - [Running the Project](#running-the-project)
  - [Verification / Testing](#verification--testing)
  - [Main Features (Highlights)](#main-features-highlights)
  - [Project Structure](#project-structure)
  - [Tips & Troubleshooting](#tips--troubleshooting)
  - [Contributing](#contributing)

  ## System Requirements
  - .NET 8 SDK (https://dotnet.microsoft.com)
  - Visual Studio 2022/2023 (with ASP.NET workload) or Visual Studio Code + C# extensions
  - SQL Server (Express/LocalDB) or compatible SQL Server instance
  - (Optional) `dotnet-ef` tool for running migrations from CLI

  ## Installation

  Notes: example commands assume PowerShell on Windows.

  1) Clone repository

  ```powershell
  git clone https://github.com/tmthanhCT/FinalAssignment-APDP.git
  cd "FinalAssignemnt_APDP"
  ```

  2) Configure connection string
  - Edit `appsettings.json` (or `appsettings.Development.json`) and set `ConnectionStrings:DefaultConnection`. Example:

  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Simester5Db;Trusted_Connection=True;Encrypt=false"
  }
  ```

  3) Restore dependencies and apply migrations (if required)

  ```powershell
  dotnet restore
  # If you don't have dotnet-ef installed
  dotnet tool install --global dotnet-ef
  dotnet ef database update
  ```

  ### Using Visual Studio Code (IDE)
  - Open the project folder in VS Code.
  - Install recommended extensions: `C#` (OmniSharp) and any .NET tooling you prefer.
  - Run the commands above in the integrated terminal to prepare the database.
  - Start the app using:

  ```powershell
  dotnet run --project .\FinalAssignemnt_APDP.csproj
  ```

  ### Using Visual Studio (VS)
  - Open `FinalAssignemnt_APDP.sln` in Visual Studio.
  - Ensure the proper run profile (IIS Express or Project) is selected.
  - Update `appsettings.json` if needed, then press F5 to run.

  ## Running the Project

  - From CLI (PowerShell):

  ```powershell
  dotnet build
  dotnet run --project .\FinalAssignemnt_APDP.csproj
  ```

  - For hot-reload during development:

  ```powershell
  dotnet watch run --project .\FinalAssignemnt_APDP.csproj
  ```

  - To publish for production:

  ```powershell
  dotnet publish -c Release -o .\publish
  ```

  After running, open the address shown in the console (for example `https://localhost:5001`).

  ## Verification / Testing
  - Build validation:

  ```powershell
  dotnet build
  ```

  - Migration & DB checks:

  ```powershell
  dotnet ef migrations list
  dotnet ef database update
  ```

  - If the repository does not include unit tests, basic verification is: build succeeds, run the app, register/login and exercise key CRUD pages.

  ## Main Features (Highlights)
  - Authentication & Authorization: registration, login, user profile management, role-based authorization (ASP.NET Identity).
  - Academic data management: CRUD for `Department`, `Major`, `Subject`, `Course`, `Semester`, and `Enrollment`.
  - User management: create/edit/delete users, assign roles (Student / Lecturer / Admin).
  - Admin dashboard: overview metrics and quick access to CRUD pages and grade management.
  - Grade management & CSV import/export: import/export CSV for grades, automatic insert/update, average score calculation and classification.
  - Lecturer workspace: timetable and class/grade summaries for lecturers.
  - UI: Blazor Server with responsive layout and sidebar navigation.

  ## Project Structure

  Key folders and files:

  ```
  FinalAssignemnt-APDP/
  ├─ Components/              # Razor components, layout, shared components
  │  ├─ Account/              # Identity-related components
  │  ├─ Layout/               # MainLayout, NavMenu
  │  └─ Pages/                # Component pages
  ├─ Pages/                   # Razor pages per module (Student, Lecturer, Admin...)
  ├─ Data/                    # Models, DbContext, Seeder, Services (UserService, etc.)
  │  ├─ ApplicationDbContext.cs
  │  ├─ ApplicationUser.cs
  │  ├─ UserService.cs
  │  └─ Migrations/           # EF Core migrations
  ├─ EndPoints/               # Minimal API / endpoint definitions
  ├─ Services/                # Business services (FileUploadService, LecturerWorkspaceService,...)
  ├─ wwwroot/                 # Static files (css, js, uploads)
  ├─ appsettings.json         # Configuration (connection strings, app settings)
  ├─ Program.cs               # DI setup, middleware, route mapping
  └─ FinalAssignemnt_APDP.csproj
  ```

  ## Tips & Troubleshooting
  - DB connection errors: confirm `ConnectionStrings` in `appsettings.json` and SQL Server access rights.
  - `dotnet ef` errors: ensure `dotnet-ef` is installed: `dotnet tool install --global dotnet-ef`.
  - HTTPS certificate issues in dev: accept the dev certificate in your browser or run the Dev Cert tool if needed.

  ## Contributing
  - Fork → create a feature/fix branch → commit → open a Pull Request.

  ---

  If you'd like, I can:
  - Add a PowerShell script to set up the database and run the app.
  - Add a seed script to create a default admin user.

