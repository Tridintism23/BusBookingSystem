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
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        List<Booking> bookingList = new List<Booking>();
        BusWindow busWindow;
        CustomerWindow customerWindow;

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
            List<Customer> customerList = customerWindow.customerList;
            List<Bus> busList = busWindow.busList;

            // Tạo thuộc tính hiển thị đẹp mắt cho ComboBox
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
            if (cbCustomers.SelectedValue == null || cbBuses.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Khách hàng và Chuyến xe!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int.TryParse(txtSeatsBooked.Text, out int seats);
            double.TryParse(txtTotalAmount.Text, out double totalAmount);

            Booking booking = new Booking();

            if (!string.IsNullOrWhiteSpace(txtBookingId.Text))
            {
                booking.BookingId = txtBookingId.Text.Trim();
            }

            // Lấy giá trị mã ccode và BusId được chọn từ ComboBox
            booking.Ccode = cbCustomers.SelectedValue.ToString();
            booking.BusId = cbBuses.SelectedValue.ToString();
            booking.SeatsBooked = seats;
            booking.TotalAmount = totalAmount;
            booking.BookingDate = DateTime.Now;

            bookingList.Add(booking);
            RefreshList();
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
            LoadDropdownData(); // Cập nhật lại luôn cả danh sách dropdown
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
