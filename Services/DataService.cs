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
    }
}
