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
        private const string CustomersFile = "customers.json";
        private const string BusesFile = "buses.json";
        private const string BookingsFile = "bookings.json";

        public List<Customer> Customers { get; set; } = new();
        public List<Bus> Buses { get; set; } = new();
        public List<Booking> Bookings { get; set; } = new();

        public static List<T> LoadFromFile<T>(string fileName)
        {
            string json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
        }

        public static void SaveToFile<T>(string fileName, List<T> data)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }

        public void LoadAllData()
        {
            Customers = LoadFromFile<Customer>(CustomersFile);
            Buses = LoadFromFile<Bus>(BusesFile);
            Bookings = LoadFromFile<Booking>(BookingsFile);
        }

        public void SaveCustomers() => SaveToFile(CustomersFile, Customers);
        public void SaveBuses() => SaveToFile(BusesFile, Buses);
        public void SaveBookings() => SaveToFile(BookingsFile, Bookings);
    
        public DataService()
        {
            LoadAllData();
            if (Customers.Count == 0)
            {
                Customers.Add(new Customer { Username = "admin", Password = "123", FullName = "System Admin"});
                Customers.Add(new Customer { Username = "customer", Password = "123", FullName = "Nguyen Van A", Phone = "0987654321", Email = "a@gmail.com"});
                SaveCustomers();
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
