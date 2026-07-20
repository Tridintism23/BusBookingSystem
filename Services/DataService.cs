using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusBookingSystem.Models;
using System.IO;
using System.Text.Json;

namespace BusBookingSystem.Services
{
    public class DataService
    {
        private const string AccountsFile = "accounts.json";
        private const string BusesFile = "buses.json";
        private const string BookingsFile = "bookings.json";

        public List<Account> Accounts { get; set; } = new();
        public List<Bus> Buses { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();

        private List<T> LoadFromFile<T>(string fileName)
        {
            if (!File.Exists(fileName)) return new List<T>();
            try
            {
                string json = File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
            }
            catch 
            {
                return new List<T>();
            }
        }

        private void SaveToFile<T>(string fileName, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        public void LoadAllData()
        {
            Accounts = LoadFromFile<Account>(AccountsFile);
            Buses = LoadFromFile<Bus>(BusesFile);
            Bookings = LoadFromFile<Booking>(BookingsFile);
        }

        public void SaveAccounts() => SaveToFile(AccountsFile, Accounts);
        public void SaveBuses() => SaveToFile(BusesFile, Buses);
        public void SaveBookings() => SaveToFile(BookingsFile, Bookings);
    
        public DataService()
        {
            LoadAllData();
            if (Accounts.Count == 0)
            {
                Accounts.Add(new Account { Username = "admin", Password = "123", FullName = "System Admin", Role = UserRole.Admin });
                Accounts.Add(new Account { Username = "customer", Password = "123", FullName = "Nguyen Van A", Phone = "0987654321", Email = "a@gmail.com", Role = UserRole.Customer });
                SaveAccounts();
            }

            if (Buses.Count == 0)
            {
                Buses.Add(new Bus { BusId = "BUS01", BusNumber = "29B-12345", Departure = "Hà Nội", Destination = "Hải Phòng", DepartureTime = "08:00 AM", TotalSeats = 40, AvailableSeats = 35, TicketPrice = 120000 });
                Buses.Add(new Bus { BusId = "BUS02", BusNumber = "30A-67890", Departure = "Hà Nội", Destination = "Đà Nẵng", DepartureTime = "07:30 PM", TotalSeats = 36, AvailableSeats = 10, TicketPrice = 350000 });
                Buses.Add(new Bus { BusId = "BUS03", BusNumber = "51B-99999", Departure = "TP.HCM", Destination = "Đà Lạt", DepartureTime = "10:00 PM", TotalSeats = 34, AvailableSeats = 28, TicketPrice = 280000 });
                SaveBuses();
            }
        }
    }
}
