using SchedulingApp.Models;
using System;
using System.IO;

namespace SchedulingApp.Business
{
    public static class SessionManager
    {
        public static User? CurrentUser { get; private set; }
        
        public static void SetCurrentUser(User user)
        {
            CurrentUser = user;
            LogLogin(user.UserName);
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static void LogLogin(string username)
        {
            try
            {
                // Simple login tracking to text file - keeps audit trail
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {username}{Environment.NewLine}";
                File.AppendAllText("Login_History.txt", logEntry);
            }
            catch (Exception ex)
            {
                // Don't let logging failure break the login process
                System.Windows.Forms.MessageBox.Show($"Error logging login: {ex.Message}");
            }
        }
    }
}
