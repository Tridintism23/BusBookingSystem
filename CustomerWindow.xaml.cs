using BusBookingSystem.Models;
using BusBookingSystem.Services;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace BusBookingSystem;

public partial class CustomerWindow : Window
{
    List<Customer> customerList = new List<Customer>();

    public CustomerWindow()
    {
        InitializeComponent();
    }

    private void Button_Search(object sender, RoutedEventArgs e)
    {

    }

    private void Button_Add_Customer(object sender, RoutedEventArgs e)
    {

    }

    private void Button_Delete(object sender, RoutedEventArgs e)
    {

    }

    private void ExportToFile(object sender, RoutedEventArgs e)
    {

    }

    private void ImportFromFile(object sender, RoutedEventArgs e)
    {
        try
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                customerList = DataService.LoadFromFile<Customer>(openFileDialog.FileName);
        } catch (Exception _)
        {
            Console.WriteLine("Can't import this file into the system");
        }
    }
}