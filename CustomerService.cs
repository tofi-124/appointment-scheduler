using MySql.Data.MySqlClient;
using SchedulingApp.Models;
using SchedulingApp.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace SchedulingApp.Business
{
    public static class CustomerService
    {
        public static List<Customer> GetAllCustomers()
        {
            try
            {
                List<Customer> customers = new List<Customer>();
                string query = @"
                    SELECT c.*, a.address, a.address2, a.postalCode, a.phone, 
                           ci.city, co.country
                    FROM customer c
                    INNER JOIN address a ON c.addressId = a.addressId
                    INNER JOIN city ci ON a.cityId = ci.cityId
                    INNER JOIN country co ON ci.countryId = co.countryId
                    WHERE c.active = 1";

                DataTable result = DatabaseConnection.ExecuteQuery(query);

                foreach (DataRow row in result.Rows)
                {
                    customers.Add(new Customer
                    {
                        CustomerId = Convert.ToInt32(row["customerId"]),
                        CustomerName = row["customerName"].ToString() ?? string.Empty,
                        AddressId = Convert.ToInt32(row["addressId"]),
                        Active = Convert.ToBoolean(row["active"]),
                        CreateDate = Convert.ToDateTime(row["createDate"]),
                        CreatedBy = row["createdBy"].ToString() ?? string.Empty,
                        LastUpdate = Convert.ToDateTime(row["lastUpdate"]),
                        LastUpdateBy = row["lastUpdateBy"].ToString() ?? string.Empty,
                        Address = row["address"].ToString() ?? string.Empty,
                        Address2 = row["address2"].ToString() ?? string.Empty,
                        City = row["city"].ToString() ?? string.Empty,
                        PostalCode = row["postalCode"].ToString() ?? string.Empty,
                        Phone = row["phone"].ToString() ?? string.Empty,
                        Country = row["country"].ToString() ?? string.Empty
                    });
                }
                return customers;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting customers: {ex.Message}");
            }
        }

        public static void AddCustomer(Customer customer)
        {
            try
            {
                ValidateCustomer(customer);

                string currentUser = SessionManager.CurrentUser?.UserName ?? "system";  // default fallback
                DateTime now = DateTime.Now;

                int countryId = GetOrCreateCountry(customer.Country, currentUser, now);
                int cityId = GetOrCreateCity(customer.City, countryId, currentUser, now);
                int addressId = CreateAddress(customer, cityId, currentUser, now);

                string query = @"
                    INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy)
                    VALUES (@customerName, @addressId, @active, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@customerName", customer.CustomerName),
                    new MySqlParameter("@addressId", addressId),
                    new MySqlParameter("@active", true),
                    new MySqlParameter("@createDate", now),
                    new MySqlParameter("@createdBy", currentUser),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser)
                };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding customer: {ex.Message}");
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            try
            {
                ValidateCustomer(customer);

                var currentUser = SessionManager.CurrentUser?.UserName ?? "system";
                var now = DateTime.Now;

                var countryId = GetOrCreateCountry(customer.Country, currentUser, now);
                var cityId = GetOrCreateCity(customer.City, countryId, currentUser, now);
                UpdateAddress(customer, cityId, currentUser, now);

                var query = @"
                    UPDATE customer 
                    SET customerName = @customerName, lastUpdate = @lastUpdate, lastUpdateBy = @lastUpdateBy
                    WHERE customerId = @customerId";

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@customerName", customer.CustomerName),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser),
                    new MySqlParameter("@customerId", customer.CustomerId)
                };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating customer: {ex.Message}");
            }
        }

        public static void DeleteCustomer(int customerId)
        {
            try
            {
                var hasAppointments = DatabaseConnection.ExecuteScalar(
                    "SELECT COUNT(*) FROM appointment WHERE customerId = @customerId",
                    new MySqlParameter[] { new MySqlParameter("@customerId", customerId) });

                if (Convert.ToInt32(hasAppointments) > 0)
                {
                    throw new Exception("Cannot delete customer with existing appointments.");
                }

                var query = "UPDATE customer SET active = 0 WHERE customerId = @customerId";
                var parameters = new MySqlParameter[] { new MySqlParameter("@customerId", customerId) };

                DatabaseConnection.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting customer: {ex.Message}");
            }
        }

        private static void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.CustomerName.Trim()))
                throw new Exception(LocalizationHelper.Translate("RequiredFields"));
            
            if (string.IsNullOrWhiteSpace(customer.Address.Trim()))
                throw new Exception(LocalizationHelper.Translate("RequiredFields"));
            
            if (string.IsNullOrWhiteSpace(customer.City.Trim()))
                throw new Exception("City is required and cannot be empty.");
            
            if (string.IsNullOrWhiteSpace(customer.Country.Trim()))
                throw new Exception("Country is required and cannot be empty.");
            
            if (string.IsNullOrWhiteSpace(customer.PostalCode.Trim()))
                throw new Exception("Postal code is required and cannot be empty.");
            
            if (string.IsNullOrWhiteSpace(customer.Phone.Trim()))
                throw new Exception(LocalizationHelper.Translate("RequiredFields"));

            if (!Regex.IsMatch(customer.Phone.Trim(), @"^[\d\-]+$"))
                throw new Exception(LocalizationHelper.Translate("InvalidPhone"));
        }

        private static int GetOrCreateCountry(string countryName, string currentUser, DateTime now)
        {
            var existingId = DatabaseConnection.ExecuteScalar(
                "SELECT countryId FROM country WHERE country = @country",
                new MySqlParameter[] { new MySqlParameter("@country", countryName) });

            if (existingId != null)
                return Convert.ToInt32(existingId);

            var newId = GetNextId("country", "countryId");
            DatabaseConnection.ExecuteNonQuery(
                "INSERT INTO country (countryId, country, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@id, @country, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)",
                new MySqlParameter[]
                {
                    new MySqlParameter("@id", newId),
                    new MySqlParameter("@country", countryName),
                    new MySqlParameter("@createDate", now),
                    new MySqlParameter("@createdBy", currentUser),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser)
                });

            return newId;
        }

        private static int GetOrCreateCity(string cityName, int countryId, string currentUser, DateTime now)
        {
            var existingId = DatabaseConnection.ExecuteScalar(
                "SELECT cityId FROM city WHERE city = @city AND countryId = @countryId",
                new MySqlParameter[]
                {
                    new MySqlParameter("@city", cityName),
                    new MySqlParameter("@countryId", countryId)
                });

            if (existingId != null)
                return Convert.ToInt32(existingId);

            var newId = GetNextId("city", "cityId");
            DatabaseConnection.ExecuteNonQuery(
                "INSERT INTO city (cityId, city, countryId, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@id, @city, @countryId, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)",
                new MySqlParameter[]
                {
                    new MySqlParameter("@id", newId),
                    new MySqlParameter("@city", cityName),
                    new MySqlParameter("@countryId", countryId),
                    new MySqlParameter("@createDate", now),
                    new MySqlParameter("@createdBy", currentUser),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser)
                });

            return newId;
        }

        private static int CreateAddress(Customer customer, int cityId, string currentUser, DateTime now)
        {
            var newId = GetNextId("address", "addressId");
            DatabaseConnection.ExecuteNonQuery(
                "INSERT INTO address (addressId, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy) VALUES (@id, @address, @address2, @cityId, @postalCode, @phone, @createDate, @createdBy, @lastUpdate, @lastUpdateBy)",
                new MySqlParameter[]
                {
                    new MySqlParameter("@id", newId),
                    new MySqlParameter("@address", customer.Address),
                    new MySqlParameter("@address2", customer.Address2),
                    new MySqlParameter("@cityId", cityId),
                    new MySqlParameter("@postalCode", customer.PostalCode),
                    new MySqlParameter("@phone", customer.Phone),
                    new MySqlParameter("@createDate", now),
                    new MySqlParameter("@createdBy", currentUser),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser)
                });

            return newId;
        }

        private static void UpdateAddress(Customer customer, int cityId, string currentUser, DateTime now)
        {
            DatabaseConnection.ExecuteNonQuery(
                "UPDATE address SET address = @address, address2 = @address2, cityId = @cityId, postalCode = @postalCode, phone = @phone, lastUpdate = @lastUpdate, lastUpdateBy = @lastUpdateBy WHERE addressId = @addressId",
                new MySqlParameter[]
                {
                    new MySqlParameter("@address", customer.Address),
                    new MySqlParameter("@address2", customer.Address2),
                    new MySqlParameter("@cityId", cityId),
                    new MySqlParameter("@postalCode", customer.PostalCode),
                    new MySqlParameter("@phone", customer.Phone),
                    new MySqlParameter("@lastUpdate", now),
                    new MySqlParameter("@lastUpdateBy", currentUser),
                    new MySqlParameter("@addressId", customer.AddressId)
                });
        }

        private static int GetNextId(string tableName, string idColumn)
        {
            var maxId = DatabaseConnection.ExecuteScalar($"SELECT COALESCE(MAX({idColumn}), 0) FROM {tableName}");
            return Convert.ToInt32(maxId) + 1;
        }
    }
}
