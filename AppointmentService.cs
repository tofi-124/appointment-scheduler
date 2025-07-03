using MySql.Data.MySqlClient;
using SchedulingApp.Models;
using SchedulingApp.Business;
using System;
using System.Collections.Generic;
using System.Data;

namespace SchedulingApp.Business
{
    public static class AppointmentService
    {
        public static List<Appointment> GetAllAppointments()
        {
            try
            {
                List<Appointment> appointmentList = new List<Appointment>();  // using explicit type here
                string query = @"
                    SELECT a.*, c.customerName, u.userName
                    FROM appointment a
                    INNER JOIN customer c ON a.customerId = c.customerId
                    INNER JOIN user u ON a.userId = u.userId";

                var result = DatabaseConnection.ExecuteQuery(query);

                foreach (DataRow row in result.Rows)
                {
                    appointmentList.Add(new Appointment
                    {
                        AppointmentId = Convert.ToInt32(row["appointmentId"]),
                        CustomerId = Convert.ToInt32(row["customerId"]),
                        UserId = Convert.ToInt32(row["userId"]),
                        Title = row["title"].ToString() ?? string.Empty,
                        Description = row["description"].ToString() ?? string.Empty,
                        Location = row["location"].ToString() ?? string.Empty,
                        Contact = row["contact"].ToString() ?? string.Empty,
                        Type = row["type"].ToString() ?? string.Empty,
                        Url = row["url"].ToString() ?? string.Empty,
                        Start = Convert.ToDateTime(row["start"]),
                        End = Convert.ToDateTime(row["end"]),
                        CreateDate = Convert.ToDateTime(row["createDate"]),
                        CreatedBy = row["createdBy"].ToString() ?? string.Empty,
                        LastUpdate = Convert.ToDateTime(row["lastUpdate"]),
                        LastUpdateBy = row["lastUpdateBy"].ToString() ?? string.Empty,
                        CustomerName = row["customerName"].ToString() ?? string.Empty,
                        UserName = row["userName"].ToString() ?? string.Empty
                    });
                }
                return appointmentList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting appointments: {ex.Message}");
            }
        }

        public static List<Appointment> GetAppointmentsByDate(DateTime date)
        {
            try
            {
                var appointments = new List<Appointment>();
                var query = @"
                    SELECT a.*, c.customerName, u.userName
                    FROM appointment a
                    INNER JOIN customer c ON a.customerId = c.customerId
                    INNER JOIN user u ON a.userId = u.userId
                    WHERE DATE(a.start) = @date";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@date", date.Date)
                };

                var result = DatabaseConnection.ExecuteQuery(query, parameters);

                foreach (DataRow row in result.Rows)
                {
                    appointments.Add(new Appointment
                    {
                        AppointmentId = Convert.ToInt32(row["appointmentId"]),
                        CustomerId = Convert.ToInt32(row["customerId"]),
                        UserId = Convert.ToInt32(row["userId"]),
                        Title = row["title"].ToString() ?? string.Empty,
                        Description = row["description"].ToString() ?? string.Empty,
                        Location = row["location"].ToString() ?? string.Empty,
                        Contact = row["contact"].ToString() ?? string.Empty,
                        Type = row["type"].ToString() ?? string.Empty,
                        Url = row["url"].ToString() ?? string.Empty,
                        Start = Convert.ToDateTime(row["start"]),
                        End = Convert.ToDateTime(row["end"]),
                        CreateDate = Convert.ToDateTime(row["createDate"]),
                        CreatedBy = row["createdBy"].ToString() ?? string.Empty,
                        LastUpdate = Convert.ToDateTime(row["lastUpdate"]),
                        LastUpdateBy = row["lastUpdateBy"].ToString() ?? string.Empty,
                        CustomerName = row["customerName"].ToString() ?? string.Empty,
                        UserName = row["userName"].ToString() ?? string.Empty
                    });
                }
                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting appointments by date: {ex.Message}");
            }
        }

        public static void AddAppointment(Appointment appointment)
        {
            try
            {
                ValidateAppointment(appointment);
                CheckForOverlappingAppointments(appointment);

                var currentUser = SessionManager.CurrentUser?.UserName ?? "system";
                var now = DateTime.Now;
                var newId = GetNextAppointmentId();

                var query = @"
                    INSERT INTO appointment (appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)
                    VALUES (@appointmentId, @customerId, @userId, @title, @description, @location, @contact, @type, @url, @start, @end, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@appointmentId", newId),
                    new MySqlParameter("@customerId", appointment.CustomerId),
                    new MySqlParameter("@userId", appointment.UserId),
                    new MySqlParameter("@title", appointment.Title),
                    new MySqlParameter("@description", appointment.Description),
                    new MySqlParameter("@location", appointment.Location),
                    new MySqlParameter("@contact", appointment.Contact),
                    new MySqlParameter("@type", appointment.Type),
                    new MySqlParameter("@url", appointment.Url),
                    new MySqlParameter("@start", appointment.Start),
                    new MySqlParameter("@end", appointment.End),
                    new MySqlParameter("@createDate", now),
                    new MySqlParameter("@createdBy", currentUser),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser)
                };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding appointment: {ex.Message}");
            }
        }

        public static void UpdateAppointment(Appointment appointment)
        {
            try
            {
                ValidateAppointment(appointment);
                CheckForOverlappingAppointments(appointment);

                var currentUser = SessionManager.CurrentUser?.UserName ?? "system";
                var now = DateTime.Now;

                var query = @"
                    UPDATE appointment 
                    SET customerId = @customerId, userId = @userId, title = @title, description = @description, 
                        location = @location, contact = @contact, type = @type, url = @url, 
                        start = @start, end = @end, lastUpdate = @lastUpdate, lastUpdateBy = @lastUpdateBy
                    WHERE appointmentId = @appointmentId";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@customerId", appointment.CustomerId),
                    new MySqlParameter("@userId", appointment.UserId),
                    new MySqlParameter("@title", appointment.Title),
                    new MySqlParameter("@description", appointment.Description),
                    new MySqlParameter("@location", appointment.Location),
                    new MySqlParameter("@contact", appointment.Contact),
                    new MySqlParameter("@type", appointment.Type),
                    new MySqlParameter("@url", appointment.Url),
                    new MySqlParameter("@start", appointment.Start),
                    new MySqlParameter("@end", appointment.End),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser),
                    new MySqlParameter("@appointmentId", appointment.AppointmentId)
                };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating appointment: {ex.Message}");
            }
        }

        public static void DeleteAppointment(int appointmentId)
        {
            try
            {
                var query = "DELETE FROM appointment WHERE appointmentId = @appointmentId";
                var parameters = new MySqlParameter[] { new MySqlParameter("@appointmentId", appointmentId) };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting appointment: {ex.Message}");
            }
        }

        public static List<Appointment> GetUpcomingAppointments(int userId, int minutes = 15)
        {
            try
            {
                var appointments = new List<Appointment>();
                var now = DateTime.Now;
                var futureTime = now.AddMinutes(minutes);

                var query = @"
                    SELECT a.*, c.customerName, u.userName
                    FROM appointment a
                    INNER JOIN customer c ON a.customerId = c.customerId
                    INNER JOIN user u ON a.userId = u.userId
                    WHERE a.userId = @userId AND a.start BETWEEN @now AND @futureTime";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@userId", userId),
                    new MySqlParameter("@now", now),
                    new MySqlParameter("@futureTime", futureTime)
                };

                var result = DatabaseConnection.ExecuteQuery(query, parameters);

                foreach (DataRow row in result.Rows)
                {
                    appointments.Add(new Appointment
                    {
                        AppointmentId = Convert.ToInt32(row["appointmentId"]),
                        CustomerId = Convert.ToInt32(row["customerId"]),
                        UserId = Convert.ToInt32(row["userId"]),
                        Title = row["title"].ToString() ?? string.Empty,
                        Description = row["description"].ToString() ?? string.Empty,
                        Location = row["location"].ToString() ?? string.Empty,
                        Contact = row["contact"].ToString() ?? string.Empty,
                        Type = row["type"].ToString() ?? string.Empty,
                        Url = row["url"].ToString() ?? string.Empty,
                        Start = Convert.ToDateTime(row["start"]),
                        End = Convert.ToDateTime(row["end"]),
                        CreateDate = Convert.ToDateTime(row["createDate"]),
                        CreatedBy = row["createdBy"].ToString() ?? string.Empty,
                        LastUpdate = Convert.ToDateTime(row["lastUpdate"]),
                        LastUpdateBy = row["lastUpdateBy"].ToString() ?? string.Empty,
                        CustomerName = row["customerName"].ToString() ?? string.Empty,
                        UserName = row["userName"].ToString() ?? string.Empty
                    });
                }
                return appointments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting upcoming appointments: {ex.Message}");
            }
        }

        private static void ValidateAppointment(Appointment appointment)
        {
            if (string.IsNullOrWhiteSpace(appointment.Title.Trim()))
                throw new Exception(LocalizationHelper.Translate("RequiredFields"));

            if (appointment.Start >= appointment.End)
                throw new Exception("Start time must be before end time.");

            var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var startInEastern = TimeZoneInfo.ConvertTime(appointment.Start, easternZone);
            var endInEastern = TimeZoneInfo.ConvertTime(appointment.End, easternZone);

            if (startInEastern.DayOfWeek == DayOfWeek.Saturday || startInEastern.DayOfWeek == DayOfWeek.Sunday ||
                endInEastern.DayOfWeek == DayOfWeek.Saturday || endInEastern.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new Exception(LocalizationHelper.Translate("BusinessHours"));
            }

            if (startInEastern.TimeOfDay < new TimeSpan(9, 0, 0) || startInEastern.TimeOfDay > new TimeSpan(17, 0, 0) ||
                endInEastern.TimeOfDay < new TimeSpan(9, 0, 0) || endInEastern.TimeOfDay > new TimeSpan(17, 0, 0))
            {
                throw new Exception(LocalizationHelper.Translate("BusinessHours"));
            }
        }

        private static void CheckForOverlappingAppointments(Appointment appointment)
        {
            var query = @"
                SELECT COUNT(*) FROM appointment 
                WHERE appointmentId != @appointmentId 
                AND ((start <= @start AND end > @start) OR (start < @end AND end >= @end) OR (start >= @start AND end <= @end))";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@appointmentId", appointment.AppointmentId),
                new MySqlParameter("@start", appointment.Start),
                new MySqlParameter("@end", appointment.End)
            };

            var count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, parameters));
            if (count > 0)
            {
                throw new Exception(LocalizationHelper.Translate("OverlappingAppointment"));
            }
        }

        private static int GetNextAppointmentId()
        {
            var maxId = DatabaseConnection.ExecuteScalar("SELECT COALESCE(MAX(appointmentId), 0) FROM appointment");
            return Convert.ToInt32(maxId) + 1;
        }
    }
}
