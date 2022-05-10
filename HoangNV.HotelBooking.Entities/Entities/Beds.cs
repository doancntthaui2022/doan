using HoangNV.HotelBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Entities.Entities
{
    public class Beds : BaseEntity
    {
        [Key]
        public int BedId { get; set; }

        [Required]
        [StringLength(255)]
        public string BedType { get; set; }

        public virtual ICollection<RoomBeds> RoomBeds { get; set; }
    }
}
