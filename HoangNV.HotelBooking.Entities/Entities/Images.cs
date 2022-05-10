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
    public class Images : BaseEntity
    {   
        [Key]
        public int ImageId { get; set; }
        public int? HotelBranchId { get; set; }
        public int? RoomTypeId { get; set; }
        public string ImageLink { get; set; }

        [ForeignKey("HotelBranchId")]
        public virtual HotelBranchs HotelBranch { get; set; }

        [ForeignKey("RoomTypeId")]
        public virtual RoomTypes RoomType { get; set; }

    }
}
