// Personal helper methods I added for better code organization
using System;
using System.Text.RegularExpressions;

namespace SchedulingApp.Utilities
{
    public static class InputHelper
    {
        // Quick phone validation - handles most common formats
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            
            // Simple regex for phone with dashes and digits
            string pattern = @"^[\d\-]+$";
            return Regex.IsMatch(phone, pattern) && phone.Replace("-", "").Length >= 10;
        }
        
        // Trim helper I use frequently
        public static string SafeTrim(string input)
        {
            return input?.Trim() ?? "";
        }
        
        // Date helper for business hours checking
        public static bool IsBusinessDay(DateTime date)
        {
            return date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
        }
    }
}
