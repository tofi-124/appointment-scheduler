# Appointment Scheduler

A Windows desktop scheduling application built with C#, WinForms, and MySQL. The core challenge this project addresses is that scheduling constraints, specifically time zone enforcement, conflict detection, and business-hours rules, are easy to get wrong when they are scattered across form event handlers. This project consolidates that logic into a dedicated service layer so the rules are testable, consistent, and independent of the UI.

This is an older project, kept here as a reference for how I approached desktop application architecture early in my career.

---

## The Problem Worth Solving

Most simple scheduling UIs move fast by embedding validation directly in button-click handlers. That works until you have multiple forms that need the same rules, or until a time zone edge case surfaces in production. The design here moves all scheduling rules into `AppointmentService` so each form is only responsible for collecting input and displaying results.

The time zone handling in particular warranted its own approach. Appointment times are stored in Eastern Time in the database and converted to the user's local time zone on read. This keeps the storage layer consistent regardless of where the application is run, while the UI always shows times that make sense to the current user.

## Architecture

The application is split into three layers with deliberate boundaries.

**UI Layer** (`LoginForm`, `MainForm`, `CustomerForm`, `AppointmentForm`, `CalendarForm`, `ReportsForm`) handles user input and display only. Forms delegate all business decisions to services and do not contain validation logic.

**Service Layer** is where the application logic lives.

- [AppointmentService.cs](AppointmentService.cs) owns scheduling rules: 9 AM to 5 PM Eastern Time, Monday through Friday, no overlapping appointments, time zone conversion on every read and write.
- [CustomerService.cs](CustomerService.cs) manages the full address hierarchy, creating city and country records on demand rather than requiring pre-population.
- [LocalizationHelper.cs](LocalizationHelper.cs) detects system culture on startup and serves translated strings for all UI labels. English and Spanish are supported out of the box.
- [SessionManager.cs](SessionManager.cs) holds the active user context and writes a timestamped login history file.

**Data Layer** ([DatabaseConnection.cs](DatabaseConnection.cs)) centralizes query execution. All queries use parameterized `MySqlParameter` arrays, not string concatenation.

## Key Engineering Decisions

**Storing times in Eastern Time rather than UTC**
The business rule is defined in Eastern Time, so storing in that zone simplifies the enforcement logic. UTC would be the better choice for a multi-region system, but for a single-timezone business constraint, this tradeoff kept the overlap detection query straightforward.

**Overlap detection at the database query level**
Rather than loading all appointments into memory and comparing in application code, overlap is checked with a direct SQL query using half-open interval logic before every insert and update. This prevents race conditions between the check and the write.

**Localization without a resource file**
Translation strings are stored in a static dictionary inside `LocalizationHelper`. A `.resx` resource file would be the standard .NET approach, but the dictionary keeps everything visible in one place and avoids the indirection of resource lookups for a two-language system.

**Address normalization**
The customer schema normalizes addresses into separate `address`, `city`, and `country` tables. `CustomerService` uses a get-or-create pattern for city and country lookups so adding a customer does not require pre-existing reference data.

## Tech Stack

- C# / .NET 8
- Windows Forms
- MySQL with MySql.Data
- LINQ for reporting aggregations

## Running Locally

1. Clone to a Windows machine with Visual Studio or the .NET 8 desktop workload installed.
2. Create the `client_schedule` MySQL database with tables: `user`, `customer`, `appointment`, `address`, `city`, `country`.
3. Update the connection string in [DatabaseConnection.cs](DatabaseConnection.cs) to point at your local MySQL instance.
4. Restore packages and build:

```bash
dotnet restore
dotnet build
dotnet run
```

## Reporting

The reports screen uses LINQ to aggregate appointment data without additional database queries. [ReportsForm.cs](ReportsForm.cs) groups and projects from the in-memory list returned by `AppointmentService` to produce three views: appointment type counts by month, per-user schedule, and a customer activity summary with last and next appointment dates.

## Known Limitations

**Hard-coded connection string.** The database credentials live in [DatabaseConnection.cs](DatabaseConnection.cs). The correct fix is to read from `App.config` or environment variables. The `App.config` in this repo has a placeholder key for this but the service class was never updated to use it.

**Plain-text password comparison.** `UserService` compares passwords directly against the database value. A production version would use a proper hashing scheme.

**Manual ID generation.** Some insert operations use `MAX(id) + 1` to generate primary keys instead of relying on auto-increment. This works at low concurrency but is not safe under concurrent writes.

**No automated tests.** The service layer is structured in a way that would make unit testing straightforward, particularly the scheduling validation and overlap detection, but no test project was added at the time.

These are acknowledged tradeoffs from the original build scope, not oversights I would repeat on a new project.