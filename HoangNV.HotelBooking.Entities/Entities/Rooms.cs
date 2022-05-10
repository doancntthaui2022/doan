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
    public class Rooms : BaseEntity
    {
        [Key]
        public int RoomId { get; set; }
        [StringLength(255)]
        public string RoomCode { get; set; }
        public int RoomTypeId { get; set; }
        public int? Status { get; set; }


        [ForeignKey("RoomTypeId")]
        public virtual RoomTypes RoomType { get; set; }

        public virtual ICollection<BookingDetails> BookingDetails { get; set; }
    }
}
