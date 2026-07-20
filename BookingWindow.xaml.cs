using System;
using System.Collections.Generic;
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
using BusBookingSystem.Models;
using BusBookingSystem.Services;

namespace BusBookingSystem
{
    /// <summary>
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        List<Booking> bookingList = new List<Booking>();

        public BookingWindow()
        {
            InitializeComponent();
            RefreshList();
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
            int.TryParse(txtSeatsBooked.Text, out int seats);
            double.TryParse(txtTotalAmount.Text, out double totalAmount);

            Booking booking = new Booking();

            // Nếu người dùng nhập BookingId thì lấy, không thì dùng Guid tự sinh
            if (!string.IsNullOrWhiteSpace(txtBookingId.Text))
            {
                booking.BookingId = txtBookingId.Text.Trim();
            }

            booking.Ccode = txtCcode.Text.Trim();
            booking.BusId = txtBusId.Text.Trim();
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
            RefreshList();
        }

        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBookings.SelectedItem is Booking selectedBooking)
            {
                txtBookingId.Text = selectedBooking.BookingId;
                txtCcode.Text = selectedBooking.Ccode;
                txtBusId.Text = selectedBooking.BusId;
                txtSeatsBooked.Text = selectedBooking.SeatsBooked.ToString();
                txtTotalAmount.Text = selectedBooking.TotalAmount.ToString();
            }
        }
    }
}
