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
    public class Convenients : BaseEntity
    {
        [Key]
        public int ConvenientId { get; set; }
        [StringLength(255)]
        public string ConvenientName { get; set; }

        public int ConvenientTypeId { get; set; }


        public virtual ICollection<RoomConvenients> RoomConvenients { get; set; }
        [ForeignKey("ConvenientTypeId")]
        public virtual ConvenientTypes ConvenientType { get; set; }
    }
}
