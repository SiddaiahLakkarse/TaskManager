# TaskManager (with Angular 22 and ASP.NET Core Web API on .NET 8)

A full-stack task management application built with Angular 22 and ASP.NET Core Web API on .NET 8. Users can register, log in securely, and manage their own tasks through a RESTful API and Angular frontend.

## Features

- User registration and login
- JWT bearer authentication with BCrypt password hashing
- Protected Angular routes and automatic JWT authorization
- Create, view, edit, complete, search, filter, and delete tasks
- User-specific task access
- Swagger/OpenAPI documentation
- Entity Framework Core migrations
- Development-time automatic database migration

## Technology stack

### Frontend

- Angular 22, TypeScript, Angular Router, and Angular Forms
- Angular Material and CDK
- SCSS, RxJS, and Vite development server
- Vitest testing framework

### Backend

- ASP.NET Core Web API on .NET 8
- C# and Entity Framework Core 8
- SQL Server LocalDB
- RESTful API architecture and dependency injection
- Swagger/OpenAPI

### Authentication

- JWT bearer tokens
- BCrypt password hashing
- Angular authentication guard
- Angular HTTP authentication interceptor

## Structure

- `backend/TaskManager.Api/Domain` - User, TaskItem, and task enums.
- `backend/TaskManager.Api/Contracts` - validated request/response DTOs.
- `backend/TaskManager.Api/Infrastructure` - EF Core persistence and JWT token service.
- `backend/TaskManager.Api/Controllers` - authentication and protected task endpoints.
- `frontend/src/app` - standalone routes, guards, interceptor, services, and UI pages.

## Prerequisites

Install the .NET 8 SDK, Node.js 20 or later, npm, SQL Server LocalDB, and Visual Studio with the ASP.NET and web development and Node.js development workloads.

Verify the installed tools:

```powershell
dotnet --version
node --version
npm --version
```

## SQL Server and backend

The development database uses SQL Server LocalDB:

```text
Server=(localdb)\\MSSQLLocalDB;
Database=TaskManagerDb;
Trusted_Connection=True;
TrustServerCertificate=True;
MultipleActiveResultSets=true
```

Connect in Visual Studio SQL Server Object Explorer using `(localdb)\\MSSQLLocalDB` and Windows Authentication. Refresh the Databases node to view `TaskManagerDb`.

The project includes an `InitialCreate` migration. When the API runs in Development, pending migrations are applied automatically. The migration creates `Users`, `Tasks`, and `__EFMigrationsHistory`.

To apply migrations manually from the repository root:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef database update --project backend/TaskManager.Api --startup-project backend/TaskManager.Api
```

The model seeds `demo@example.com` / `Demo1234!` and one sample task. Change this password before using the sample beyond development.

## Running the application

### Visual Studio

1. Open `TaskManager.sln`.
2. Set `TaskManager.Api` as the startup project.
3. Select the `https` launch profile.
4. Press **F5**.
5. Open `http://localhost:4200`.

The ASP.NET Core SPA proxy starts the Angular development server automatically.

### Manual startup

Run the backend:

```powershell
dotnet run --project backend/TaskManager.Api --launch-profile https
```

Run the frontend in a second terminal:

```powershell
cd frontend
npm install
npm start -- --host localhost --port 4200
```

The Angular API URL is configured in `frontend/src/environments/environment.ts`. Change `apiUrl` if the API uses another port. The checked-in CORS policy allows `http://localhost:4200`.

Open `http://localhost:4200`. Accept the local HTTPS certificate warning when calling the API if prompted.

## Application URLs

| Component | URL or value |
|---|---|
| Angular frontend | `http://localhost:4200` |
| ASP.NET Core API | `https://localhost:7001` |
| Swagger | `https://localhost:7001/swagger` |
| SQL Server instance | `(localdb)\\MSSQLLocalDB` |
| Database | `TaskManagerDb` |

## API examples

Register:

```http
POST https://localhost:7001/api/auth/register
Content-Type: application/json

{"name":"Ada Lovelace","email":"ada@example.com","password":"Password123!"}
```

Login, then choose **Authorize** in Swagger at `https://localhost:7001/swagger` and enter `Bearer <token>`:

```http
POST https://localhost:7001/api/auth/login
Content-Type: application/json

{"email":"demo@example.com","password":"Demo1234!"}
```

Protected requests include `Authorization: Bearer <token>`:

```http
GET /api/tasks?status=InProgress&priority=High&search=explore
POST /api/tasks
{"title":"Ship MVP","description":"Finish the release","status":"ToDo","priority":"High","dueDate":"2026-12-31T00:00:00Z"}
PATCH /api/tasks/{id}/complete
PUT /api/tasks/{id}
DELETE /api/tasks/{id}
```

## Design decisions

JWT claims carry the user ID; every task query and mutation scopes by that claim, preventing cross-user access. EF Core owns persistence and migrations, while controllers remain thin. Angular keeps the token in a dedicated storage service, attaches it only through an interceptor, and protects the dashboard route with a functional guard. API URL and secrets are configuration-driven rather than embedded in application logic.

## Testing and build

Run frontend tests:

```powershell
cd frontend
npm test
```

Build the backend:

```powershell
dotnet build backend/TaskManager.Api/TaskManager.Api.csproj
```

## Default development account

```text
Email: demo@example.com
Password: Demo1234!
```

## Security notes

The JWT key in `appsettings.json` is for local development only and must not be reused in production. Before production deployment:

- Replace the development JWT key.
- Store secrets in environment variables, .NET User Secrets, Azure Key Vault, or another secure store.
- Use a production SQL Server instance instead of LocalDB.
- Restrict CORS to approved frontend domains.
- Enforce HTTPS and use least-privilege database credentials.
- Never commit production passwords, tokens, or connection secrets.

## GitHub repository description

> A full-stack task management application built with Angular 22, ASP.NET Core Web API on .NET 8, Entity Framework Core, SQL Server LocalDB, JWT authentication, and BCrypt password hashing.

Suggested GitHub topics: `angular`, `typescript`, `dotnet`, `aspnet-core`, `web-api`, `entity-framework-core`, `sql-server`, `localdb`, `jwt-authentication`, `rest-api`, `task-management`, `full-stack`.
