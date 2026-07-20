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
using System.IO;
using System.Text.Json;

namespace BusBookingSystem
{
    /// <summary>
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        List<Booking> bookingList = new List<Booking>();
        BusWindow busWindow;
        CustomerWindow customerWindow;
        private List<Bus> currentBusList = new List<Bus>();

        public BookingWindow(BusWindow busWindow, CustomerWindow customerWindow)
        {
            this.busWindow = busWindow;
            this.customerWindow = customerWindow;
            InitializeComponent();
        }

        public void RefreshWindow()
        {
            LoadDropdownData();
            RefreshList();

        }

        // Nạp dữ liệu cho 2 ComboBox Khách hàng và Chuyến xe
        private void LoadDropdownData()
        {
            List<Customer> customerList = customerWindow?.customerList ?? new List<Customer>();
            List<Bus> busList = busWindow?.busList ?? new List<Bus>();
            

            if (customerList.Count == 0 && System.IO.File.Exists("customers.json"))
            {
                string json = File.ReadAllText("customers.json");
                var loadedCustomers = JsonSerializer.Deserialize<List<Customer>>(json);
                if (loadedCustomers != null && loadedCustomers.Count > 0)
                {
                    customerList = loadedCustomers;
                    if (customerWindow != null) customerWindow.customerList = customerList;
                }
            }

            if (busList.Count == 0 && System.IO.File.Exists("buses.json"))
            {
                string json = File.ReadAllText("buses.json");
                var loadedBuses = JsonSerializer.Deserialize<List<Bus>>(json);
                if (loadedBuses != null && loadedBuses.Count > 0)
                {
                    busList = loadedBuses;
                    if (busWindow != null) busWindow.busList = busList;
                }
            }

            cbCustomers.ItemsSource = customerList.Select(c => new
            {
                ccode = c.ccode,
                DisplayInfo = $"{c.ccode} - {c.name}"
            }).ToList();

            cbBuses.ItemsSource = busList.Select(b => new
            {
                BusId = b.BusId,
                DisplayInfo = $"{b.BusId} ({b.Departure} -> {b.Destination})"
            }).ToList();

            currentBusList = busList;
        }

        private void RefreshList()
        {
            dgBookings.ItemsSource = null;
            dgBookings.ItemsSource = bookingList;
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
                var filteredList = bookingList.Where(b =>
                    b.Ccode.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    b.BusId.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    b.BookingId.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                dgBookings.ItemsSource = filteredList;
            }
        }

        private void btnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomers.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Khách hàng!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbBuses.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Chuyến xe!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string bookingId = txtBookingId.Text.Trim();

            if (string.IsNullOrWhiteSpace(bookingId))
            {
                MessageBox.Show("Vui lòng nhập Booking ID!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (bookingList.Any(b => b.BookingId.Equals(bookingId, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show($"Mã đặt vé (Booking ID) '{bookingId}' đã tồn tại! Vui lòng nhập mã khác.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtSeatsBooked.Text, out int seats) || seats <= 0)
            {
                MessageBox.Show("Số lượng ghế đặt phải là một số nguyên dương lớn hơn 0!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedBusId = cbBuses.SelectedValue.ToString();
            Bus selectedBus = currentBusList.FirstOrDefault(b => b.BusId == selectedBusId);

            if (selectedBus == null)
            {
                MessageBox.Show("Không tìm thấy thông tin chuyến xe đã chọn trong hệ thống!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (seats > selectedBus.AvailableSeats)
            {
                MessageBox.Show($"Chuyến xe này chỉ còn {selectedBus.AvailableSeats} ghế trống! Bạn không thể đặt {seats} ghế.",
                                "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            double calculatedTotalAmount = seats * selectedBus.TicketPrice;

            Booking booking = new Booking
            {
                BookingId = bookingId,
                Ccode = cbCustomers.SelectedValue.ToString(),
                BusId = selectedBusId,
                SeatsBooked = seats,
                TotalAmount = calculatedTotalAmount,
                BookingDate = DateTime.Now
            };

            selectedBus.AvailableSeats -= seats;

            bookingList.Add(booking);
            RefreshList();

            cbBuses.ItemsSource = currentBusList.Select(b => new
            {
                BusId = b.BusId,
                DisplayInfo = $"{b.BusId} ({b.Departure} -> {b.Destination}) - {b.TicketPrice:N0} VNĐ (Còn: {b.AvailableSeats} ghế)"
            }).ToList();

            ClearFormInputs();

            MessageBox.Show("Đặt vé thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearFormInputs()
        {
            txtBookingId.Clear();
            cbCustomers.SelectedIndex = -1;
            cbBuses.SelectedIndex = -1;
            txtSeatsBooked.Text = "1";
            txtTotalAmount.Text = "";
        }

        private void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is Booking selectedBooking)
            {
                bookingList.Remove(selectedBooking);
                RefreshList();
            }
        }

        private void btnWriteFile_Click(object sender, RoutedEventArgs e)
        {
            DataService.SaveToFile("BookingList", bookingList);
        }

        private void btnReadFile_Click(object sender, RoutedEventArgs e)
        {
            bookingList = DataService.LoadFromFile<Booking>();
            LoadDropdownData();
            RefreshList();
        }

        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBookings.SelectedItem is Booking selectedBooking)
            {
                txtBookingId.Text = selectedBooking.BookingId;
                cbCustomers.SelectedValue = selectedBooking.Ccode;
                cbBuses.SelectedValue = selectedBooking.BusId;
                txtSeatsBooked.Text = selectedBooking.SeatsBooked.ToString();
                txtTotalAmount.Text = selectedBooking.TotalAmount.ToString();
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
