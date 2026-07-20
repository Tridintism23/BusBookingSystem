using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusBookingSystem.Services;

namespace BusBookingSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window busWindow;
        private Window customerWindow;
        private Window bookingWindow;

        public MainWindow()
        {
            busWindow = new BusWindow();
            customerWindow = new CustomerWindow();
            bookingWindow = new BookingWindow((BusWindow)busWindow, (CustomerWindow)customerWindow);
            InitializeComponent();
        }

        private void ButtonBus(object sender, RoutedEventArgs e)
        {
            busWindow.ShowDialog();
        }

        private void ButtonCustomer(object sender, RoutedEventArgs e)
        {
            customerWindow.ShowDialog();
        }

        private void ButtonBooking(object sender, RoutedEventArgs e)
        {
            ((BookingWindow)bookingWindow).RefreshWindow();
            bookingWindow.ShowDialog();
        }
    }
}