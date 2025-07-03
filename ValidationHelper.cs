// Small utility methods I added for convenience
using System;

namespace SchedulingApp.Helpers
{
    public static class ValidationHelper
    {
        // Quick phone validation - just checks basic format
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrEmpty(phone)) return false;
            
            // Remove spaces and check format
            phone = phone.Replace(" ", "").Replace("-", "");
            return phone.Length >= 10 && phone.Length <= 15;
        }
        
        // Helper to clean up input strings
        public static string CleanInput(string input)
        {
            return input?.Trim() ?? "";
        }
    }
}
