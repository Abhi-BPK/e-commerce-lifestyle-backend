# EcommerceLifestyle Backend

ASP.NET Core 8 Web API for the men's-clothing e-commerce site.
Three-layer architecture (API → BLL → DAL) with **Repository + Unit-of-Work** patterns, Entity Framework Core (Pomelo MySQL provider), and JWT auth.

The React frontend lives in the sibling folder `e-commerce-lifestyle-frontend/` and is **not modified** by this project.

---

## Project layout

```
e-commerce-lifestyle-backend/
├── EcommerceLifestyle.sln
├── global.json                       (pins SDK to .NET 10 with rollForward)
├── EcommerceLifestyle.Api/           Presentation layer
│   ├── Controllers/
│   ├── Middleware/                   ErrorHandlingMiddleware, RequestLoggingMiddleware
│   ├── Program.cs                    DI wiring + pipeline
│   └── appsettings.json
├── EcommerceLifestyle.BLL/           Business-logic layer
│   ├── Dtos/
│   ├── Interfaces/
│   ├── Services/
│   ├── Mapping/
│   └── Exceptions/
└── EcommerceLifestyle.DAL/           Data-access layer
    ├── Entities/
    ├── Persistence/                  AppDbContext, SeedData
    ├── Interfaces/                   IRepository<T>, IUnitOfWork, etc.
    └── Repositories/                 Repository<T>, UnitOfWork, etc.
```

Reference flow: **API → BLL → DAL** (one-way only).

---

## Prerequisites

- .NET 8 (or 10) SDK
- MySQL 8.x running on **port 3307** with `root` / `root`
  - Quick docker spin-up: `docker run -d --name ecom-mysql -p 3307:3306 -e MYSQL_ROOT_PASSWORD=root mysql:8`
- `dotnet-ef` global tool: `dotnet tool install --global dotnet-ef`

---

## Run for the first time

```bash
cd e-commerce-lifestyle-backend
dotnet build
dotnet ef migrations add Initial -p EcommerceLifestyle.DAL -s EcommerceLifestyle.Api
dotnet ef database update         -p EcommerceLifestyle.DAL -s EcommerceLifestyle.Api
dotnet run --project EcommerceLifestyle.Api
```

Open Swagger at <http://localhost:5000/swagger>.

The seed will populate **36 products** (6 per subcategory) plus 36 inventory rows.

---

## Default endpoints

| Method | Path                                  | Auth                  |
|--------|---------------------------------------|-----------------------|
| POST   | /api/auth/login                       | anonymous             |
| POST   | /api/auth/signup                      | anonymous             |
| GET    | /api/products/men?subcategory={slug}  | anonymous             |
| GET    | /api/products/men/{id}                | anonymous             |
| POST/PUT/DELETE /api/products[/{id}]   | Vendor                |
| GET/POST/PUT/DELETE /api/cart[/{id}]   | Authenticated         |
| GET/POST/PUT/DELETE /api/inventory[/{id}] | Vendor             |
| GET/POST /api/orders[/{id}]            | Authenticated         |
| GET    /api/users/me                   | Authenticated         |
| PUT    /api/users/me                   | Authenticated         |
| GET    /api/logs?level=&since=&take=   | Vendor                |

All errors return a uniform JSON shape: `{ status, message, field?, code? }` --
this matches what the React frontend's `errorNormalizer` already expects.

---

## Useful tips

- **Reset the database**: `dotnet ef database drop -p EcommerceLifestyle.DAL -s EcommerceLifestyle.Api -f`
- **Add a migration**: `dotnet ef migrations add MigrationName -p EcommerceLifestyle.DAL -s EcommerceLifestyle.Api`
- **Run from the API folder**: `dotnet run --project EcommerceLifestyle.Api`
