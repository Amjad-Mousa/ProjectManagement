# 🗂️ Project Management Website

This project is a **Project Management Web Application** built using **.NET**. It demonstrates **Clean Architecture** principles and includes authentication, authorization, task & project management, observability, logging, and Docker support.

---

## ✨ Key Features

* **Authentication & Authorization:** .NET Cookie-based Authentication and Role-based Authorization.
* **Projects CRUD:** Add, Update, Delete (owner + admin), Get, Get All My Projects, Get All Projects (Admin only).
* **Tasks CRUD:** Add, Update, Delete (owner + admin), Get, Get All Project Tasks, Get All Tasks (Admin only).
* **Database:** SQL Server as the backend.
* **Architecture:** Structured according to **Clean Architecture** with layers: Domain, Application, Infrastructure, API.
* **Error Handling:** Global Exception Handling for consistent error responses.
* **User Context:** `IUserContext` interface to access currently logged-in user info in the Application layer.
* **Observability:** OpenTelemetry for tracing, logging, and metrics exported to a .NET Aspire container.
* **Health Checks:** Standard Health Checks (database connectivity, liveness, readiness).
* **Docker Setup:** Run the app with Visual Studio + Docker (API, SQL Server, Aspire).
* **Logging:** .NET built-in logging for key events and business operations.
* **iMapper:** Auto-mapping between entities and DTOs.

---

## 🛠️ Tech Stack

| Category             | Technologies/Tools                                       |
| :------------------- | :------------------------------------------------------- |
| **Backend**          | .NET 5.0+, Clean Architecture, LINQ (async)              |
| **Database**         | SQL Server (SSMS), EF Core Migrations                    |
| **API**              | Controllers, Cookie-based Auth, Role-based Authorization |
| **Mapping**          | iMapper                                                  |
| **Error Handling**   | Global Exception Middleware                              |
| **Observability**    | OpenTelemetry (tracing, logging, metrics)                |
| **Health Checks**    | Built-in ASP.NET Core Health Checks                      |
| **Containerization** | Docker, Docker Compose                                   |
| **Testing**          | Postman Collection, Swagger                              |
| **Other**            | Git, GitHub                                              |

---

## 📦 Project Structure

The solution is organized to cleanly separate concerns:

```text
ProjectManagement.Solution/
├── Domain/                 # Core Models
│   ├── Models/
├── Application/            # Application logic
│   ├── Services/
│   ├── DTOs/
│   ├── Commands/
│   ├── Queries/
│   └── Interfaces/         # IUserContext, repositories
├── Infrastructure/         # Data access, EF Core, DB context, repositories
│   ├── Data/
│   ├── Repositories/
│   └── Mappings/           # iMapper profiles
├── API/                    # Web API project
│   ├── Controllers/
│   ├── Middleware/
│   ├── Extensions/
│   ├── Program.cs & Startup.cs
│   └── Dockerfile
├── .gitignore
├── ProjectManagement.sln
└── docker-compose.yml
```

---

## 💾 Database Schema

**Entities & Relationships:**

| Entity   | Key Attributes                                                              | Notes                     |
| :------- | :-------------------------------------------------------------------------- | :------------------------ |
| Projects | ProjectId (PK), Name, Description, OwnerId (FK), CreatedAt                  | Owner + admin can delete  |
| Tasks    | TaskId (PK), ProjectId (FK), Name, Description, AssignedTo, Status, DueDate | 1-to-many Project → Tasks |
| Users    | UserId (PK), Username, Email, Role                                          | Role: Admin, User         |
| Other    | —                                                                           | TBD                       |

---

## 🚀 Setup Instructions

**Prerequisites:**

* .NET 5.0+ SDK
* SQL Server + SSMS
* Git
* Docker & Docker Compose
* Postman (for testing)

**1. Clone & Restore**

```bash
git clone https://github.com/yourusername/ProjectManagement.git
cd ProjectManagement
dotnet restore
```

**2. Database Setup**

* Create a database in SSMS.
* Update connection string in `Infrastructure/appsettings.json`.
* Apply EF Core migrations:

```bash
dotnet ef migrations add InitialCreate --project Infrastructure
dotnet ef database update --project Infrastructure
```

**3. Run API (Local)**

```bash
cd API
dotnet run
```

* API: `https://localhost:5001`
* Swagger: `https://localhost:5001/swagger`

**4. Docker Deployment**

```bash
docker-compose up --build
```

* API → `http://localhost:5000/swagger`
* DB persists in `./docker-data`

---

## 🔗 API Endpoints

**Projects**

| Method | Endpoint           | Description                   |
| :----- | :----------------- | :---------------------------- |
| POST   | /api/projects      | Add project                   |
| PUT    | /api/projects/{id} | Update project                |
| DELETE | /api/projects/{id} | Delete project (owner/admin)  |
| GET    | /api/projects/{id} | Get project                   |
| GET    | /api/projects/my   | Get my projects               |
| GET    | /api/projects      | Get all projects (Admin only) |

**Tasks**

| Method | Endpoint                             | Description                |
| :----- | :----------------------------------- | :------------------------- |
| POST   | /api/projects/{projectId}/tasks      | Add task                   |
| PUT    | /api/projects/{projectId}/tasks/{id} | Update task                |
| DELETE | /api/projects/{projectId}/tasks/{id} | Delete task (owner/admin)  |
| GET    | /api/projects/{projectId}/tasks/{id} | Get task                   |
| GET    | /api/projects/{projectId}/tasks      | Get all project tasks      |
| GET    | /api/tasks                           | Get all tasks (Admin only) |

**Auth**

| Method | Endpoint           | Description |
| :----- | :----------------- | :---------- |
| POST   | /api/auth/login    | Login       |
| POST   | /api/auth/register | Register    |

---

## 🔒 Validation & Error Handling

* **FluentValidation** for model validation
* **Global Exception Middleware** → unified error responses
* **Logging** → structured logging with .NET built-in abstractions

---

## 🧪 Testing

* Manual via Swagger UI
* Postman Collection included in `/docs`
* Automated via Newman:

```bash
newman run ProjectManagementAPI.postman_collection.json -e dev.env
```

---

## 📈 Observability

* **OpenTelemetry** → metrics + tracing
* **Metrics endpoint** exposed via `/metrics`

---

## 🤝 Contributing

* Fork repository
* Create a feature branch: `git checkout -b feature/your-feature-name`
* Commit with semantic messages: `feat: ...`
* Push branch & open PR to `master`
