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
    public class RoomBeds : BaseEntity
    {
        [Key]
        public int RoomBedId { get; set; }
        public int RoomTypeId { get; set; }
        public int BedId { get; set; }
        public int NumOfBed { get; set; }


        [ForeignKey("RoomTypeId")]
        public virtual RoomTypes RoomType { get; set; }
        [ForeignKey("BedId")]
        public virtual Beds Bed { get; set; }
    }
}
