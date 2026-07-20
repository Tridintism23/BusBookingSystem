using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusBookingSystem.Models
{
    public class Booking
    {
        public string BookingId { get; set; } = Guid.NewGuid().ToString().Substring(0, 8);
        public string Ccode { get; set; } = string.Empty;
        public string BusId { get; set; } = string.Empty;
        public int SeatsBooked { get; set; }
        public DateTime BookingDate { get; set; } = DateTime.Now;
        public double TotalAmount { get; set; }
    }
}
