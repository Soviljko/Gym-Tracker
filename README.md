
# Gym Tracker

A gym workout tracking application. 
Users can register, log in,
record their workouts (type, duration, calories, intensity, fatigue, notes), and view their progress broken down
by week for any selected month.

## Tech stack

**Backend**
- ASP.NET Core Web API (.NET 9)
- Clean Architecture: Domain / Application / Infrastructure / Api
- Entity Framework Core with Pomelo (MySQL provider)
- JWT bearer authentication
- Swagger / OpenAPI (Swashbuckle)
- xUnit for unit tests

**Frontend**
- Angular 21 (standalone components, zoneless change detection)
- Reactive forms
- Plain CSS (no component library) with a centralized color palette via CSS variables

**Database**
- MySQL 8.4, run via Docker Compose

## Project structure

```
backend/
  GymTracker.sln
  src/
    GymTracker.Domain          -> entities, enums (no external dependencies)
    GymTracker.Application     -> DTOs, interfaces, business logic services
    GymTracker.Infrastructure  -> EF Core, migrations, repositories, JWT/hashing
    GymTracker.Api             -> controllers, DI wiring, middleware
  tests/
    GymTracker.Application.Tests
frontend/
  src/app/
    guards/            -> route auth guard
    interceptors/       -> JWT auth interceptor
    services/            -> Auth/Workout/Progress services
    pages/               -> login, register, workouts, progress
docker-compose.yaml
```

Dependencies point inward only: `Api` depends on `Application` and `Infrastructure`; `Infrastructure` depends on
`Application`; `Application` depends on `Domain`; `Domain` depends on nothing. Controllers never talk to the
database directly — they call Application-layer services, which use repository interfaces implemented in
Infrastructure.

## Running the project

### Prerequisites
- .NET 9 SDK
- Node.js 20+ and Angular CLI
- Docker Desktop

### 1. Database

From the repository root:

```bash
docker compose up -d
```

This starts a MySQL 8.4 container (`gymtracker-mysql`), exposed on host port **3307** (mapped to the
container's 3306, to avoid clashing with a locally installed MySQL on the default port). Credentials and the
database name are defined in `docker-compose.yaml` (root / devpassword / gymtracker) — fine for local
development, not meant for production use.

### 2. Backend

```bash
cd backend
dotnet ef database update --project src/GymTracker.Infrastructure --startup-project src/GymTracker.Api
cd src/GymTracker.Api
dotnet run
```

The API listens on `http://localhost:5195`. Swagger UI is at `http://localhost:5195/swagger` — use the
**Authorize** button to paste a JWT (obtained from `/api/auth/login`) and test protected endpoints directly.

### 3. Frontend

```bash
cd frontend
npm install
ng serve
```

Open `http://localhost:4200`. The app expects the API at `http://localhost:5195/api`
(see `frontend/src/environments/environment.ts`).

### 4. Running tests

```bash
cd backend
dotnet test
```

## API overview

| Method | Endpoint | Auth | Description |
|---|---|---|---|
| POST | `/api/auth/register` | — | Create an account, returns a JWT |
| POST | `/api/auth/login` | — | Returns a JWT |
| GET | `/api/workouts` | JWT | List the current user's workouts |
| POST | `/api/workouts` | JWT | Create a workout |
| PUT | `/api/workouts/{id}` | JWT | Update a workout |
| DELETE | `/api/workouts/{id}` | JWT | Delete a workout |
| GET | `/api/progress?year=&month=` | JWT | Weekly summary (total duration, workout count, average intensity/fatigue) for the given month |

All endpoints except register/login require an `Authorization: Bearer <token>` header. Workouts are always
scoped to the authenticated user; requesting or modifying another user's workout returns `404` (not `403`, to
avoid revealing whether the resource exists).

## Key decisions

- **.NET 9**: the SDK available locally; the architecture is version-agnostic (retargeting is a one-line change).
- **MySQL** over SQLite/Postgres: chosen to get hands-on practice with it. Runs via Docker Compose so the
  project stays "clone → up → run" regardless of the reviewer's machine.
- **Weekly aggregation**: ISO calendar weeks (Monday–Sunday). Every week overlapping the selected month is
  shown, but only workouts whose date is actually inside that month count toward its totals.
- **No component library or chart** in the UI: the spec only asks for a display of the numbers per week, which
  a plain table satisfies. Time went into backend architecture and correctness instead.
- **Workout editing**: the API fully supports `PUT`, but the UI only exposes add/delete (update is testable via
  Swagger).
- **Tests**: scoped to `WeeklySummaryCalculator`, the only piece of real algorithmic logic in the codebase.
  Everything else is a thin pass-through to EF Core / ASP.NET, verified manually end-to-end instead.
  
## Application Demo Video

https://github.com/user-attachments/assets/b6d39cf1-2946-41f1-97ce-7fe4f59dc2be




