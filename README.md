# WGU C969 Scheduling Application
# STUDENT ID tmoha34

Hello! This is my scheduling application project for the WGU Software Development course. It's a Windows desktop app that helps manage customers and appointments with all the required features.



### Database Setup
**Database Credentials Used:**
- Host: 127.0.0.1
- Database: client_schedule
- Username: sqlUser  
- Password: Passw0rd!

### All Requirment Below Have Been Met

**Main Features:**
- Login screen that detects your location and shows in English or Spanish
- Add, edit, and delete customers 
- Schedule appointments with time zone handling
- Calendar view to see all appointments
- Alerts when you have appointments coming up in 15 minutes or no appointments
- Reports showing appointment summaries and schedules

**Rules Applied:**
- Appointments can only be scheduled 9 AM to 5 PM, Monday through Friday (Eastern Time)
- No overlapping appointments allowed
- All times get converted to Eastern Time for business hours checking
- Login attempts are saved to a "Login_History.txt" file located at where the application is running

## Other Evaluation Criterias Met 

- Lambda expressions are used in the reports
- Exception handling throughout the application
- Input validation on all forms
- Time zone conversions for appointment scheduling
- Localization support with automatic detection
- All CRUD operations for customers and appointments