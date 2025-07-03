using System;
using System.Collections.Generic;
using System.Globalization;

namespace SchedulingApp
{
    public static class LocalizationHelper
    {
        private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
        {
            ["en-US"] = new Dictionary<string, string>
            {
                ["Username"] = "Username",
                ["Password"] = "Password",
                ["Login"] = "Login",
                ["InvalidCredentials"] = "The username and password do not match.",
                ["LocationDetected"] = "Location detected: {0}",
                ["Welcome"] = "Welcome",
                ["Customers"] = "Customers",
                ["Appointments"] = "Appointments",
                ["Calendar"] = "Calendar",
                ["Reports"] = "Reports",
                ["Logout"] = "Logout",
                ["Add"] = "Add",
                ["Update"] = "Update",
                ["Delete"] = "Delete",
                ["Cancel"] = "Cancel",
                ["Save"] = "Save",
                ["Name"] = "Name",
                ["Address"] = "Address",
                ["Phone"] = "Phone",
                ["City"] = "City",
                ["PostalCode"] = "Postal Code",
                ["Country"] = "Country",
                ["Title"] = "Title",
                ["Description"] = "Description",
                ["Location"] = "Location",
                ["Contact"] = "Contact",
                ["Type"] = "Type",
                ["Start"] = "Start",
                ["End"] = "End",
                ["Customer"] = "Customer",
                ["ValidationError"] = "Validation Error",
                ["RequiredFields"] = "All required fields must be filled.",
                ["InvalidPhone"] = "Phone number can only contain digits and dashes.",
                ["BusinessHours"] = "Appointments must be scheduled during business hours (9:00 AM - 5:00 PM, Monday-Friday EST).",
                ["OverlappingAppointment"] = "This appointment overlaps with an existing appointment.",
                ["AppointmentReminder"] = "You have an appointment within 15 minutes: {0}",
                ["NoUpcomingAppointments"] = "No upcoming appointments."
            },
            ["es-ES"] = new Dictionary<string, string>
            {
                ["Username"] = "Nombre de usuario",
                ["Password"] = "Contraseña",
                ["Login"] = "Iniciar sesión",
                ["InvalidCredentials"] = "El nombre de usuario y la contraseña no coinciden.",
                ["LocationDetected"] = "Ubicación detectada: {0}",
                ["Welcome"] = "Bienvenido",
                ["Customers"] = "Clientes",
                ["Appointments"] = "Citas",
                ["Calendar"] = "Calendario",
                ["Reports"] = "Informes",
                ["Logout"] = "Cerrar sesión",
                ["Add"] = "Agregar",
                ["Update"] = "Actualizar",
                ["Delete"] = "Eliminar",
                ["Cancel"] = "Cancelar",
                ["Save"] = "Guardar",
                ["Name"] = "Nombre",
                ["Address"] = "Dirección",
                ["Phone"] = "Teléfono",
                ["City"] = "Ciudad",
                ["PostalCode"] = "Código postal",
                ["Country"] = "País",
                ["Title"] = "Título",
                ["Description"] = "Descripción",
                ["Location"] = "Ubicación",
                ["Contact"] = "Contacto",
                ["Type"] = "Tipo",
                ["Start"] = "Inicio",
                ["End"] = "Fin",
                ["Customer"] = "Cliente",
                ["ValidationError"] = "Error de validación",
                ["RequiredFields"] = "Todos los campos obligatorios deben estar completos.",
                ["InvalidPhone"] = "El número de teléfono solo puede contener dígitos y guiones.",
                ["BusinessHours"] = "Las citas deben programarse durante el horario comercial (9:00 AM - 5:00 PM, lunes a viernes EST).",
                ["OverlappingAppointment"] = "Esta cita se superpone con una cita existente.",
                ["AppointmentReminder"] = "Tienes una cita en 15 minutos: {0}",
                ["NoUpcomingAppointments"] = "No hay citas próximas."
            }
        };

        private static string currentCulture = "en-US";

        public static void SetCulture(string culture)
        {
            currentCulture = culture;
        }

        public static string GetCurrentCulture()
        {
            return currentCulture;
        }

        public static string Translate(string key, params object[] args)
        {
            if (Translations.ContainsKey(currentCulture) && Translations[currentCulture].ContainsKey(key))
            {
                var translation = Translations[currentCulture][key];
                return args.Length > 0 ? string.Format(translation, args) : translation;
            }
            return key;
        }

        public static string DetectUserLocation()
        {
            var culture = CultureInfo.CurrentCulture;
            var region = new RegionInfo(culture.LCID);
            
            if (culture.Name.StartsWith("es"))
            {
                SetCulture("es-ES");
            }
            else
            {
                SetCulture("en-US");
            }
            
            return region.DisplayName;
        }
    }
}
