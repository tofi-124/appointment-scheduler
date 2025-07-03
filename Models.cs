using System;

namespace SchedulingApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
    }

    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int AddressId { get; set; }
        public bool Active { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
        
        // Additional display properties for convenience
        public string Address { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }

    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
    }

    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
    }

    public class Address
    {
        public int AddressId { get; set; }
        public string AddressLine1 { get; set; } = string.Empty;
        public string AddressLine2 { get; set; } = string.Empty;
        public int CityId { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime LastUpdate { get; set; }
        public string LastUpdateBy { get; set; } = string.Empty;
    }
}
