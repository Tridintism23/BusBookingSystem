using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusBookingSystem.Models;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;

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

        public static List<T> LoadFromFile<T>()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "JSON (*.json)|*.json";
                if (openFileDialog.ShowDialog() == true)
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    return JsonSerializer.Deserialize<List<T>>(json) ?? new List<T>();
                }
            } catch (Exception _)
            {
                Console.WriteLine("Can't import this file into the system");
            }
            return [];
        }

        public static void SaveToFile<T>(string fileName, List<T> data)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                saveFileDialog.Title = fileName;
                saveFileDialog.DefaultExt = "json";
                saveFileDialog.Filter = "JSON (*.json)|*.json";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(saveFileDialog.FileName, json);
                }
            } catch (Exception _)
            {
                Console.WriteLine("Error occurs when saving to file");
            }
        }

        public void LoadAllData()
        {
            Accounts = LoadFromFile<Account>();
            Buses = LoadFromFile<Bus>();
            Bookings = LoadFromFile<Booking>();
        }

        public void SaveAccounts() => SaveToFile(AccountsFile, Accounts);
        public void SaveBuses() => SaveToFile(BusesFile, Buses);
        public void SaveBookings() => SaveToFile(BookingsFile, Bookings);
    
        public DataService()
        {
            //LoadAllData();
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
