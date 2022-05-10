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
    public class Customers : BaseEntity
    {
        [Key]
        public int CustomerId { get; set; }
        [StringLength(255)]
        public string CustomerName { get; set; }
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        [StringLength(50)]
        public string CheckInPersonCode { get; set; } //Số cmnd
        [StringLength(255)]
        public string CheckInPersonName { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
