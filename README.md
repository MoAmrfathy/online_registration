# Online Registration System

## Overall

The **Online Registration System** is a web-based application that allows students to register for courses offered each semester. It provides an intuitive interface for students to view available courses, submit enrollment requests, and track their registration status. Admins can manage course offerings, schedules, and student registrations efficiently.

## Master Branch (Blazor WebAssembly App)

1. Built using **Blazor WebAssembly** (.NET).
2. Provides login functionality for students and admins (without JWT authentication).
3. Students can browse, filter, and enroll in available courses.
4. Admins can manage course lists, enrollment requests, and schedules.
5. Real-time feedback and validation on course enrollment actions.

## Backend API Branch (ASP.NET Core Web API)

1. Developed using **ASP.NET Core Web API**.
2. Structured using a layered architecture: Controller → Service → Repository.
3. Uses Entity Framework Core for database operations and migrations.
4. Supports full CRUD for courses, students, and enrollment records.
5. Ensures data integrity with proper validation and business logic layers.

## Technologies Used

### Programming Languages:
1. **C#** (.NET for frontend and backend)
2. **HTML/CSS** (Blazor UI components)

### Frameworks & Libraries:
1. **Blazor WebAssembly** – Frontend framework.
2. **ASP.NET Core Web API** – Backend services.
3. **Entity Framework Core** – ORM for database interaction.
4. **Bootstrap** – UI styling and layout.

### Tools:
1. **Visual Studio 2022** – Development environment.
2. **Postman** – API testing and verification.
3. **SQL Server Management Studio (SSMS)** – Database management.
4. **Draw.io / Microsoft Visio** – For ERD and system diagram design.
5. **Git & GitHub** – Version control and source collaboration.

## Features

- ✅ Student registration and course enrollment functionality  
- ✅ Admin management for course creation and updates  
- ✅ Enrollment history tracking  
- ✅ Real-time validation of available seats during enrollment

## Installation

### Prerequisites

Before running this project, ensure that you have the following installed:

- **.NET 6.0 SDK** (or later)
- **SQL Server** (or use a local SQL Server instance)

### Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/YourUsername/Online-Course-Registration-System.git
   cd Online-Course-Registration-System
   ```

2. **Restore the NuGet packages**:
   ```bash
   dotnet restore
   ```

3. **Set up the database**:
   - Update the `appsettings.json` file with your SQL Server connection string.

4. **Apply database migrations**:
   ```bash
   dotnet ef database update
   ```

5. **Build and run the application**:
   ```bash
   dotnet run
   ```

## Documentation

📄 **All system diagrams and supporting documentation** are included within the project folder:

- The **ERD (Entity Relationship Diagram)** is available in the `docs` directory.

## Contributing

I appreciate community contributions! Here's how you can help:

1. Fork the repository.
2. Create a new feature branch: `git checkout -b feature/my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin feature/my-new-feature`
5. Submit a pull request.

## Contact

📬 **Project Contact**  
Mohamed Amr  
[My LinkedIn](https://www.linkedin.com/in/mohamed-fathy-97a916351/)  
moamrfathytawfik@gmail.com
