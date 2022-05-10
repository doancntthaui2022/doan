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
    public class BookingDetails : BaseEntity
    {
        [Key]
        public string BookingDetailId { get; set; }
        public string BookingId { get; set; }
        public int RoomId { get; set; }
        public decimal CostNow { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        [ForeignKey("RoomId")]
        public virtual Rooms Rooms { get; set; }
    }
}
