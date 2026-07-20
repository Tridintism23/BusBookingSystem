using BusBookingSystem.Models;
using BusBookingSystem.Services;
using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BusBookingSystem;

public partial class CustomerWindow : Window
{
    public List<Customer> customerList { private set; get; }  = new List<Customer>();

    public CustomerWindow()
    {
        InitializeComponent();
        RefreshList();
    }
    
    private void RefreshList()
    {
        CustomerDG.ItemsSource = customerList;
        CustomerDG.Items.Refresh();
    }

    private void Button_Search(object sender, RoutedEventArgs e)
    {
        List<Customer> searchList = new List<Customer>();
        foreach (Customer customer in customerList)
        {
            if (customer.name.Contains(SearchTB.Text ?? "")) {
                searchList.Add(customer);
            }
        }
        CustomerDG.ItemsSource = searchList;
    }
    private void Button_Add_Customer(object sender, RoutedEventArgs e)
    {
        Customer customer = new Customer();
        customer.ccode = CCodeTB.Text;
        customer.name = NameTB.Text;
        customer.phone = PhoneTB.Text;
        if (customerList.Exists(c => c.ccode.Equals(customer.ccode))) return;
        customerList.Add(customer);
        RefreshList();
    }

    private void Button_Delete(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        foreach (Customer customer in customerList)
        {
            if (customer.ccode.Equals(button.Tag))
            {
                customerList.Remove(customer);
                break;
            }   
        }
        RefreshList();
    }

    private void ExportToFile(object sender, RoutedEventArgs e)
    {
        DataService.SaveToFile("CustomerList", customerList);
    }

    private void ImportFromFile(object sender, RoutedEventArgs e)
    {
        customerList = DataService.LoadFromFile<Customer>();
        RefreshList();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        this.Visibility = Visibility.Hidden;
        e.Cancel = true;
    }
}