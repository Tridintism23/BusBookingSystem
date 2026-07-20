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
        private Window busWindow = new BusWindow();
        private Window customerWindow = new CustomerWindow();
        private Window bookingWindow = new Window();

        public MainWindow()
        {
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
            bookingWindow.ShowDialog();
        }
    }
}