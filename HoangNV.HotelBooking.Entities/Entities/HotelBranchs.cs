using HoangNV.HotelBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Entities.Entities
{
    public class HotelBranchs : BaseEntity
    {
        [Key]
        public int HotelBranchId { get; set; }
        [StringLength(255)]
        public string HotelBranchName { get; set; }
        [StringLength(255)]
        public string HotelBranchCode { get; set; }
        [StringLength(255)]
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Images> Images { get; set; }
        public virtual ICollection<RoomTypes> RoomTypes { get; set; }
    }
}
