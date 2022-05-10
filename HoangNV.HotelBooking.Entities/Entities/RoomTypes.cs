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
    public class RoomTypes : BaseEntity
    {
        [Key]
        public int RoomTypeId { get; set; }
        [StringLength(255)]
        public string RoomTypeCode { get; set; }


        [StringLength(255)]
        public string RoomTypeName { get; set; }
        public decimal Cost { get; set; }
        public string Description { get; set; }
        public float Area { get; set; }
        public int NumOfPer { get; set; }

        public int HotelBranchId { get; set; }

        [ForeignKey("HotelBranchId")]
        public virtual HotelBranchs HotelBranch { get; set; }
        public virtual ICollection<Rooms> Rooms { get; set; }
        public virtual ICollection<RoomBeds> RoomBeds { get; set; }
        public virtual ICollection<RoomConvenients> RoomConvenients { get; set; }
        public virtual ICollection<Images> Images { get; set; }

    }
}
