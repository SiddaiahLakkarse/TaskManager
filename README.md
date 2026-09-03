# TaskManager

A full-stack task manager sample: Angular standalone components + Angular Material frontend and ASP.NET Core 8 Web API + Entity Framework Core + SQL Server backend.

## Structure

- `backend/TaskManager.Api/Domain` - User, TaskItem, and task enums.
- `backend/TaskManager.Api/Contracts` - validated request/response DTOs.
- `backend/TaskManager.Api/Infrastructure` - EF Core persistence and JWT token service.
- `backend/TaskManager.Api/Controllers` - authentication and protected task endpoints.
- `frontend/src/app` - standalone routes, guards, interceptor, services, and UI pages.

## Prerequisites

Install .NET 8 SDK or later, Node.js 20+, SQL Server (Developer/Express/localDB), and the Angular CLI (`npm install -g @angular/cli`).

## SQL Server and backend

1. Create or start a SQL Server instance. The default development connection uses Windows authentication and trusts the local development certificate.
2. Update `backend/TaskManager.Api/appsettings.json` (or use user secrets/environment variables) with `ConnectionStrings:DefaultConnection` and a long random `Jwt:Key`.
3. From the repository root run:

```powershell
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project backend/TaskManager.Api --startup-project backend/TaskManager.Api
dotnet ef database update --project backend/TaskManager.Api --startup-project backend/TaskManager.Api
dotnet run --project backend/TaskManager.Api --urls https://localhost:7001
```

The model seeds `demo@example.com` / `Demo1234!` and one sample task. Change this password before using the sample beyond development.

## Frontend

The Angular environment is in `frontend/src/environments/environment.ts`; change `apiUrl` if the API uses another port. The checked-in CORS policy allows `http://localhost:4200`.

```powershell
cd frontend
npm install
npm start -- --host localhost --port 4200
```

Open `http://localhost:4200`. Accept the local HTTPS certificate warning when calling the API, or configure the API to use HTTP during development and update the environment URL.

## Run both projects with Visual Studio

1. Complete the SQL Server and database setup above.
2. Open `TaskManager.sln` in Visual Studio.
3. In Solution Explorer, right-click `TaskManager.Api` and choose **Set as Startup Project**. Do not start the `frontend` project separately; the ASP.NET Core SPA proxy starts it automatically.
4. Select the backend `https` launch profile.
5. Press **F5**.

Visual Studio starts the ASP.NET Core API on `https://localhost:7001` and automatically starts the Angular development server with `npm start` on `http://localhost:4200`. The browser opens the frontend. Stop debugging to stop both processes. The first F5 run may take a moment while the frontend dependencies are restored; run `npm install` in `frontend` first if needed.

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
