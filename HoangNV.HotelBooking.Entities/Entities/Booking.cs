using HoangNV.HotelBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Entities.Entities
{
    public class Booking : BaseEntity
    {
        [Key]
        public string BookingId { get; set; }
        public int BookingStatus { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public decimal SumCost { get; set; }
        public int CustomerId { get; set; }
        public virtual ICollection<BookingDetails> BookingDetails { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customers Customer { get; set; }
    }
}
