using BusBookingSystem.Models;
using BusBookingSystem.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BusBookingSystem
{
    /// <summary>
    /// Interaction logic for BusWindow.xaml
    /// </summary>
    public partial class BusWindow : Window
    {
        public List<Bus> busList { private set; get; } = new List<Bus>();

        public BusWindow()
        {
            InitializeComponent();
            RefreshList();
        }

        private void RefreshList()
        {
            dgBuses.ItemsSource = null;
            dgBuses.ItemsSource = busList;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                RefreshList();
            }
            else
            {
                var filteredList = busList.Where(b =>
                    b.Destination.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    b.Departure.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                dgBuses.ItemsSource = filteredList;
            }
        }

        private void btnAddBus_Click(object sender, RoutedEventArgs e)
        {
            int.TryParse(txtTotalSeats.Text, out int totalSeats);
            int.TryParse(txtAvailableSeats.Text, out int availableSeats);
            double.TryParse(txtTicketPrice.Text, out double price);

            Bus bus = new Bus();
            bus.BusId = txtBusId.Text;
            bus.BusNumber = txtBusNumber.Text;
            bus.Departure = txtDeparture.Text;
            bus.Destination = txtDestination.Text;
            bus.DepartureTime = txtDepartureTime.Text;
            bus.TotalSeats = totalSeats;
            bus.AvailableSeats = availableSeats;
            bus.TicketPrice = price;

            busList.Add(bus);
            RefreshList();
        }

        private void btnDeleteBus_Click(object sender, RoutedEventArgs e)
        {
            if (dgBuses.SelectedItem is Bus selectedBus)
            {
                busList.Remove(selectedBus);
                RefreshList();
            }
        }

        private void btnWriteFile_Click(object sender, RoutedEventArgs e)
        {
            DataService.SaveToFile("BusList", busList);
        }

        private void btnReadFile_Click(object sender, RoutedEventArgs e)
        {
            busList = DataService.LoadFromFile<Bus>();
            RefreshList();
        }

        private void dgBuses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBuses.SelectedItem is Bus selectedBus)
            {
                txtBusId.Text = selectedBus.BusId;
                txtBusNumber.Text = selectedBus.BusNumber;
                txtDeparture.Text = selectedBus.Departure;
                txtDestination.Text = selectedBus.Destination;
                txtDepartureTime.Text = selectedBus.DepartureTime;
                txtTotalSeats.Text = selectedBus.TotalSeats.ToString();
                txtAvailableSeats.Text = selectedBus.AvailableSeats.ToString();
                txtTicketPrice.Text = selectedBus.TicketPrice.ToString();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
