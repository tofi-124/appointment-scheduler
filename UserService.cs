using MySql.Data.MySqlClient;
using SchedulingApp.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace SchedulingApp.Business
{
    public static class UserService
    {
        public static User? ValidateUser(string username, string password)
        {
            try
            {
                // Query to check user credentials
                string query = "SELECT * FROM user WHERE userName = @username AND password = @password AND active = 1";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@username", username),
                    new MySqlParameter("@password", password)
                };

                DataTable result = DatabaseConnection.ExecuteQuery(query, parameters);

                if (result.Rows.Count > 0)
                {
                    DataRow row = result.Rows[0];
                    return new User
                    {
                        UserId = Convert.ToInt32(row["userId"]),
                        UserName = row["userName"].ToString() ?? string.Empty,
                        Password = row["password"].ToString() ?? string.Empty,
                        Active = Convert.ToBoolean(row["active"]),
                        CreateDate = Convert.ToDateTime(row["createDate"]),
                        CreatedBy = row["createdBy"].ToString() ?? string.Empty,
                        LastUpdate = Convert.ToDateTime(row["lastUpdate"]),
                        LastUpdateBy = row["lastUpdateBy"].ToString() ?? string.Empty
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error validating user: {ex.Message}");
            }
        }

        public static List<User> GetAllUsers()
        {
            try
            {
                var users = new List<User>();
                var query = "SELECT * FROM user WHERE active = 1";
                var result = DatabaseConnection.ExecuteQuery(query);

                foreach (DataRow row in result.Rows)
                {
                    users.Add(new User
                    {
                        UserId = Convert.ToInt32(row["userId"]),
                        UserName = row["userName"].ToString() ?? string.Empty,
                        Password = row["password"].ToString() ?? string.Empty,
                        Active = Convert.ToBoolean(row["active"]),
                        CreateDate = Convert.ToDateTime(row["createDate"]),
                        CreatedBy = row["createdBy"].ToString() ?? string.Empty,
                        LastUpdate = Convert.ToDateTime(row["lastUpdate"]),
                        LastUpdateBy = row["lastUpdateBy"].ToString() ?? string.Empty
                    });
                }
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting users: {ex.Message}");
            }
        }
    }
}
