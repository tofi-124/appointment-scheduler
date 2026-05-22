# Appointment Scheduler

Appointment Scheduler is a Windows desktop application built with C#, WinForms, and MySQL for managing customers, appointments, calendar views, and reporting workflows. The project showcases business-rule validation, time zone aware scheduling, localization, and a service-driven structure in a practical desktop CRUD application.

## Overview

This application supports a complete scheduling workflow from login through reporting. Users can manage customer records, create and update appointments, review schedules in local time, and generate summary reports from live relational data.

## Technical Highlights

- Multi-form WinForms application with a clear separation between UI, services, and models
- Customer and appointment CRUD operations backed by MySQL
- Business-hours enforcement for appointments scheduled between 9:00 AM and 5:00 PM Eastern Time, Monday through Friday
- Appointment overlap detection before inserts and updates are committed
- Automatic English and Spanish login experience based on the user's system culture
- Upcoming appointment reminders during login
- Calendar view that displays appointments in the user's local time zone
- LINQ-powered reports for appointment volume, user schedules, and customer activity
- Parameterized SQL queries throughout the data access layer

## Core Workflows

- Authenticate users and start a session-aware desktop workflow
- Create, edit, and deactivate customer records
- Manage appointments with business-rule validation
- Convert appointment times between local time and Eastern Time for scheduling rules
- Review daily appointments in a calendar interface
- Generate reports for appointment types by month, schedules by user, and customer appointment summaries

## Architecture

### UI Layer

- `LoginForm` handles authentication, localization, and upcoming appointment alerts
- `MainForm` serves as the application hub for customers, appointments, calendar, and reports
- `CustomerForm` manages customer CRUD workflows
- `AppointmentForm` manages appointment CRUD workflows and local-time display
- `CalendarForm` provides day-based schedule browsing
- `ReportsForm` builds summary views from appointment and customer data

### Service Layer

- `AppointmentService` handles validation, overlap detection, time conversion, and appointment queries
- `CustomerService` manages customer data plus related address, city, and country records
- `UserService` validates users and loads active user records
- `SessionManager` stores the current user and writes login history

### Supporting Components

- `DatabaseConnection` centralizes MySQL query execution
- `LocalizationHelper` provides English and Spanish translations
- `Models.cs` defines the core domain models used across the application

## Tech Stack

- C#
- .NET 8 Windows Forms
- MySQL
- MySql.Data
- LINQ

## Local Setup

1. Use a Windows environment with Visual Studio or the .NET desktop tooling required for WinForms development.
2. Create or restore the `client_schedule` MySQL database and its related tables.
3. Update the database connection in `DatabaseConnection.cs` so it matches your local MySQL environment.
4. Restore NuGet packages.
5. Run the solution from Visual Studio.

## What This Project Demonstrates

- Turning business constraints into validation logic
- Handling scheduling rules across time zones
- Structuring a desktop application around reusable service classes
- Querying and shaping relational data for reporting with LINQ
- Building a desktop user flow with multiple coordinated forms

## Notes

- This project targets Windows because it is built with WinForms.
- Login activity is written to `Login_History.txt` in the application's output directory.
- The repository reflects the original project implementation and is a good base for future refinements such as externalized configuration and stronger authentication practices.