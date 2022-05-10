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
    public class RoomConvenients : BaseEntity
    {
        [Key]
        public int RoomConvenientId { get; set; }
        public int RoomTypeId { get; set; }
        public int ConvenientId { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomTypes RoomType { get; set; }
        [ForeignKey("ConvenientId")]
        public virtual Convenients Convenient { get; set; }
    }
}
