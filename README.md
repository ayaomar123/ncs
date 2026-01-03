# Nationwide Care Solutions (NCS) — MVP (Phase 1)

This repo contains the **first MVP** for the Nationwide Care Solutions (NCS) charity website:

- **Backend**: ASP.NET Core Web API (.NET 8), Clean Architecture, CQRS (MediatR), EF Core + SQL Server (code-first migrations), FluentValidation, Serilog, Swagger, JWT admin auth
- **Frontend**: Angular (standalone, feature-based) + Tailwind CSS, mobile-first and accessible forms

> MVP note: **No real payments**. We only store **donation intents** and redirect to a success page. A payment provider integration is planned for Phase 2.

---

## 1) MVP Sprint Plan (high-level)

1. **Foundation & architecture**
   - Create Clean Architecture solution structure (Domain/Application/Infrastructure/WebApi)
   - Add logging (Serilog), global exception handling, Swagger
2. **Persistence**
   - Create EF Core `NcsDbContext`
   - Configure entities + value conversions (tags/gallery)
   - Add repository implementations
   - Seed default admin + sample content
3. **Public APIs**
   - Appeals list/details (paging + filters)
   - Blog list/details (paging + tag filter)
   - Contact form submission
   - Donation intent creation
4. **Admin APIs**
   - Admin login (JWT)
   - CRUD for Appeals + Blog posts
   - Image upload endpoint (local storage)
5. **Angular MVP UI**
   - Public pages + placeholder routes
   - Admin dashboard + basic CRUD screens
   - Auth interceptor + admin guard
6. **Polish**
   - Accessibility + mobile-first layouts
   - Error handling + validation messages

---

## 2) Project Structure

```
.
├─ backend/
│  ├─ NCS.sln
│  └─ src/
│     ├─ NCS.Domain/               # Entities + Enums only
│     ├─ NCS.Application/          # CQRS use-cases, DTOs, interfaces, validation
│     ├─ NCS.Infrastructure/       # EF Core DbContext, repository implementations, local file storage
│     └─ NCS.WebApi/               # Controllers, DI setup, middleware, auth, swagger
└─ frontend/
   └─ ncs-web/                     # Angular + Tailwind
      └─ src/app/
         ├─ core/                  # services/interceptors/guards/models
         ├─ features/              # feature modules (public + admin)
         └─ shared/                # shared UI components
```

---

## 3) Seed Data

Seed data is added in:

- `backend/src/NCS.Infrastructure/Persistence/DbSeeder.cs`

It seeds:

- **1 default admin user** (from configuration)
- **2 appeals** (published)
- **2 blog posts** (published)

---

## 4) Run Locally

### Prerequisites

- .NET SDK 8
- Node 20+
- SQL Server (e.g. local instance or Docker)

### Backend

1. Configure SQL connection and secrets in:
   - `backend/src/NCS.WebApi/appsettings.json`

2. Create the first migration (code-first):

```bash
cd backend/src/NCS.WebApi

dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate --project ../NCS.Infrastructure --startup-project .

dotnet ef database update --project ../NCS.Infrastructure --startup-project .
```

3. Run the API:

```bash
cd backend/src/NCS.WebApi

dotnet run
```

- Swagger: `http://localhost:5000/swagger`

### Frontend

```bash
cd frontend/ncs-web
npm install
npm start
```

- App: `http://localhost:4200`

---

## 5) API Overview

### Public

- `GET /api/appeals?pageNumber=&pageSize=&isUrgent=&countryTag=`
- `GET /api/appeals/{slug}`
- `GET /api/posts?pageNumber=&pageSize=&tag=`
- `GET /api/posts/{slug}`
- `POST /api/contact`
- `POST /api/donations` → returns `{ donationRequestId, redirectUrl }`

### Admin (JWT)

- `POST /api/admin/auth/login` → `{ token }`
- `GET/POST/PUT/DELETE /api/admin/appeals`
- `GET/POST/PUT/DELETE /api/admin/posts`
- `POST /api/admin/media/upload` (multipart form-data)

---

## Phase 2 Placeholder: Payments

- `NCS.Application/Interfaces/Payments/IPaymentProvider.cs`
- `NCS.Infrastructure/Services/PaymentProviderStub.cs`

No Stripe (or any provider) is implemented in MVP.
