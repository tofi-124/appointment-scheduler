# Appointment Scheduler

Appointment Scheduler is a Windows desktop scheduling system built with C#, WinForms, and MySQL. It manages customers, appointments, calendar views, and reports while enforcing real scheduling constraints such as business hours, appointment conflicts, time zone conversion, and localized login flows.

## Project Snapshot

- Platform: Windows desktop
- Framework: .NET 8 WinForms
- Data store: MySQL
- Focus areas: business-rule validation, time zone aware scheduling, localization, reporting

## Why This Project

Scheduling software becomes unreliable when time zones, overlapping appointments, and fixed business-hour rules are handled inconsistently. This project was built to address those problems with a validation-first workflow and a service layer that keeps scheduling rules separate from the UI.

## Feature Highlights

- Customer and appointment CRUD workflows backed by MySQL
- Automatic English and Spanish login experience based on the user's system culture
- Appointment reminders for meetings starting within 15 minutes
- Calendar view that presents appointments in the user's local time zone
- LINQ-based reporting for appointment volume, user schedules, and customer activity
- Parameterized SQL queries throughout the data access layer

## Business Rules Implemented

- Appointments must fall between 9:00 AM and 5:00 PM Eastern Time
- Appointments must be scheduled Monday through Friday
- Overlapping appointments are rejected before save
- Appointment times are converted between local time and Eastern Time for scheduling logic
- Login activity is recorded in a history file for traceability

## Tech Stack

- C#
- .NET 8
- Windows Forms
- MySQL
- MySql.Data
- LINQ

## Running Locally

1. Clone the repository to a Windows machine with Visual Studio or .NET desktop tooling installed.
2. Create or restore the `client_schedule` MySQL database.
3. Make sure the database includes the `user`, `customer`, `appointment`, `address`, `city`, and `country` tables.
4. Update the connection string in [DatabaseConnection.cs](DatabaseConnection.cs) to match your local MySQL environment.
5. Restore NuGet packages.
6. Open the solution in Visual Studio and run the application.

If you prefer the .NET CLI, use:

```bash
dotnet restore
dotnet build
dotnet run
```

## Application Workflow

1. Sign in through a localized login screen.
2. Review upcoming appointments after authentication.
3. Manage customer records used by the scheduling workflow.
4. Create or update appointments with automatic validation.
5. Browse daily schedules through the calendar view.
6. Generate reports for monthly appointment types, user schedules, and customer summaries.

## Code Highlights

- [AppointmentService.cs](AppointmentService.cs) contains the core scheduling rules, overlap detection, and time zone conversion logic.
- [CustomerService.cs](CustomerService.cs) handles customer CRUD plus related address, city, and country records.
- [LocalizationHelper.cs](LocalizationHelper.cs) manages culture detection and translated UI text.
- [ReportsForm.cs](ReportsForm.cs) uses LINQ to shape reporting data for the desktop UI.
- [SessionManager.cs](SessionManager.cs) tracks the active user and writes login history.

## What This Project Demonstrates

- Translating business rules into enforceable application logic
- Structuring a desktop application around forms, services, and domain models
- Working with relational data in a desktop CRUD workflow
- Converting and presenting time-sensitive data across time zones
- Building reporting features with LINQ over domain data

## Future Improvements

- Move database configuration out of source code and into environment-specific settings
- Add automated tests for scheduling validation and reporting logic
- Replace plain-text credential handling with stronger authentication practices
- Add screenshots or a short demo to show the desktop workflows visually