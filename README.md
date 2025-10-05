# JokeWebApp

JokeWebApp is an ASP.NET Core 8.0 MVC app that lets registered users create, browse, edit, delete, and search for jokes. Authentication is provided through ASP.NET Core Identity, while data is stored locally in a SQLite database for a cross-platform developer experience.

## Features
- CRUD management UI for jokes with Identity-protected pages available out of the box
- Search form that filters jokes by their question text
- ASP.NET Core Identity scaffolding configured to require confirmed accounts before sign-in
- Entity Framework Core migrations targeting SQLite (file-based `Jokes.db` database)

## Tech Stack
- .NET 8.0
- ASP.NET Core MVC & Razor Pages
- Entity Framework Core with the SQLite provider
- ASP.NET Core Identity

## Project Structure
```
JokeWebApp
├── Areas
│   └── Identity
│       └── Pages/             # Identity Razor Pages
├── Controllers
│   ├── Api/
│   │   └── JokesApiController.cs
│   ├── HomeController.cs
│   └── JokesController.cs
├── Data
│   ├── ApplicationDbContext.cs
│   └── SeedData.cs            # Seeds starter jokes
├── Migrations
│   ├── 20251005185343_InitialCreate.cs
│   ├── 20251005185343_InitialCreate.Designer.cs
│   └── ApplicationDbContextModelSnapshot.cs
├── Models
│   ├── ErrorViewModel.cs
│   └── Joke.cs
├── Views
│   ├── Home/
│   │   ├── Index.cshtml
│   │   └── Privacy.cshtml
│   ├── Jokes/
│   │   ├── Create.cshtml
│   │   ├── Delete.cshtml
│   │   ├── Details.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Index.cshtml
│   │   └── ShowSearchForm.cshtml
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   ├── _Layout.cshtml.css
│   │   ├── _LoginPartial.cshtml
│   │   ├── _ValidationScriptsPartial.cshtml
│   │   └── Error.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot
│   ├── css/site.css
│   ├── js/site.js
│   └── lib/                # Bundled frontend libraries (bootstrap, jquery, validation)
├── Program.cs             # App bootstrap & middleware configuration
├── appsettings.json       # Base configuration (SQLite connection)
├── appsettings.Development.json
└── JokeWebApp.csproj      # Project definition and NuGet packages
```

## Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- (Optional) `sqlite3` command-line tools if you want to inspect the database file manually
- (Optional) Visual Studio 2022 or Visual Studio Code for debugging support

## Getting Started
1. **Clone or download** this repository.
2. **Restore dependencies** from the project root:
   ```bash
   dotnet restore
   ```
3. **Configure the connection string** (if needed):
   - Development defaults to `Data Source=Jokes.db` in `appsettings.Development.json` (or `appsettings.json`).
   - To place the SQLite file elsewhere, change the `DefaultConnection` value or use environment variables such as `ConnectionStrings__DefaultConnection`.

## Database Setup
Entity Framework Core migrations are included. Running them will create or update the local `Jokes.db` file.

```bash
# Apply migrations to create/update the SQLite database
dotnet ef database update
```

> **Tooling:** If `dotnet ef` is not available, install the CLI tool once via `dotnet tool install --global dotnet-ef`.

To add new migrations after changing the data model:

```bash
# Example: create a migration after editing models/dB context
dotnet ef migrations add AddNewFeature
```

If you need a clean database, delete `Jokes.db` and rerun `dotnet ef database update`.

## Running the Application Locally
Start the site from the repository root:

```bash
dotnet run
```

The app redirects to HTTPS. Open the `https://localhost:{port}` shown in the console. Identity requires confirmed accounts; for quick local trials you can temporarily disable that requirement by editing `Program.cs`:

```csharp
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
});
```

Remember to restore the confirmation requirement before deploying.

### Swagger / OpenAPI
When the app runs in the Development environment, Swagger is enabled automatically. After starting the app, browse to:

- Swagger UI: `https://localhost:{port}/swagger`
- Raw OpenAPI JSON: `https://localhost:{port}/swagger/v1/swagger.json`

If you deploy to another environment and want Swagger available there, add `app.UseSwagger()` and `app.UseSwaggerUI()` to the non-development branch in `Program.cs`.

### REST API Endpoints
Swagger exposes CRUD endpoints under `JokesApi` for the jokes catalogue:

- `GET /api/JokesApi` – list all jokes
- `GET /api/JokesApi/{id}` – fetch a single joke by id
- `POST /api/JokesApi` – create a new joke
- `PUT /api/JokesApi/{id}` – update an existing joke (payload id must match the route id)
- `DELETE /api/JokesApi/{id}` – remove a joke

Use the Swagger UI "Try it out" buttons to send requests. The seeded jokes are inserted automatically on first launch, making it easy to test immediately. When making `POST`/`PUT` calls, provide JSON bodies matching the `Joke` schema (e.g., `{"jokeQuestion": "...", "jokeAnswer": "..."}`).

## Deployment
1. **Publish** for your target environment:
   ```bash
   dotnet publish -c Release
   ```
   Output lives in `bin/Release/net8.0/publish/`.
2. **Configure environment variables** on the host for `ASPNETCORE_ENVIRONMENT` and `ConnectionStrings__DefaultConnection` (use an absolute SQLite path or another provider).
3. **Run migrations** against the deployed database:
   ```bash
   dotnet ef database update --connection "Data Source=/path/to/Jokes.db"
   ```
4. **Host** the published output (IIS, Azure App Service, Docker, etc.). Ensure HTTPS and static files are enabled. If you switch to another database engine (e.g., SQL Server or PostgreSQL), update the EF Core provider package, connection string, and `Use...` call in `Program.cs` accordingly.

## Troubleshooting
- **Database locked errors:** SQLite allows a single writer; close other processes or tools accessing `Jokes.db` during migration or updates.
- **Missing `dotnet ef`:** Install the CLI tool as noted earlier or run migrations via `dotnet run` using `IDesignTimeDbContextFactory` if you add one.
- **HTTPS certificate prompts:** Trust the development certificate with `dotnet dev-certs https --trust`.
- **Email confirmation flow:** Implement an email sender (e.g., SMTP, SendGrid) or disable confirmation for local testing only.

## Contributing & Testing
No automated tests are shipped yet. Consider adding xUnit or MSTest projects for controller/service coverage before submitting changes.

Issues and pull requests with enhancements are welcome.
