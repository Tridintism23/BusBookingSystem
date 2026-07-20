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
    /// Interaction logic for BusWindow.xaml
    /// </summary>
    public partial class BusWindow : Window
    {
        private readonly DataService _dataService;

        public BusWindow()
        {
            InitializeComponent();
            _dataService = new DataService();

            // Nạp dữ liệu vào DataGrid
            RefreshDataGrid();
        }

        // --- HÀM HỖ TRỢ REFRESH DỮ LIỆU ---
        private void RefreshDataGrid()
        {
            dgBuses.ItemsSource = null;
            dgBuses.ItemsSource = _dataService.Buses;
        }

        // --- 1. TÌM KIẾM ---
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(keyword))
            {
                RefreshDataGrid();
            }
            else
            {
                var filtered = _dataService.Buses.Where(b =>
                    b.Destination.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    b.Departure.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                dgBuses.ItemsSource = filtered;
            }
        }

        // --- 2. THÊM CHUYẾN XE MỚI ---
        private void btnAddBus_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtBusId.Text) || string.IsNullOrWhiteSpace(txtBusNumber.Text))
            {
                MessageBox.Show("Vui lòng nhập Bus ID và Bus number!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int.TryParse(txtTotalSeats.Text, out int totalSeats);
            int.TryParse(txtAvailableSeats.Text, out int availSeats);
            double.TryParse(txtTicketPrice.Text, out double price);

            Bus newBus = new Bus
            {
                BusId = txtBusId.Text.Trim(),
                BusNumber = txtBusNumber.Text.Trim(),
                Departure = txtDeparture.Text.Trim(),
                Destination = txtDestination.Text.Trim(),
                DepartureTime = txtDepartureTime.Text.Trim(),
                TotalSeats = totalSeats > 0 ? totalSeats : 40,
                AvailableSeats = availSeats >= 0 ? availSeats : totalSeats,
                TicketPrice = price
            };

            _dataService.Buses.Add(newBus);
            _dataService.SaveBuses();

            RefreshDataGrid();
            ClearInputs();
            MessageBox.Show("Thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // --- 3. XÓA CHUYẾN XE ---
        private void btnDeleteBus_Click(object sender, RoutedEventArgs e)
        {
            if (dgBuses.SelectedItem is Bus selectedBus)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa bus {selectedBus.BusId}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _dataService.Buses.Remove(selectedBus);
                    _dataService.SaveBuses();
                    RefreshDataGrid();
                    ClearInputs();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 chuyến xe trong bảng để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // --- 4. ĐỌC TỪ FILE (READ FROM FILE) ---
        private void btnReadFile_Click(object sender, RoutedEventArgs e)
        {
            _dataService.LoadAllData();
            RefreshDataGrid();
            MessageBox.Show("Đã đọc lại dữ liệu từ file JSON!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // --- 5. GHI RA FILE (WRITE TO FILE) ---
        private void btnWriteFile_Click(object sender, RoutedEventArgs e)
        {
            _dataService.SaveBuses();
            MessageBox.Show("Đã lưu dữ liệu ra file buses.json thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // --- CHỌN 1 DÒNG TRÊN DATAGRID SẼ TỰ HIỂN THỊ LÊN FORM ---
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

        private void ClearInputs()
        {
            txtBusId.Text = string.Empty;
            txtBusNumber.Text = string.Empty;
            txtDeparture.Text = string.Empty;
            txtDestination.Text = string.Empty;
            txtDepartureTime.Text = string.Empty;
        }
    }
}
