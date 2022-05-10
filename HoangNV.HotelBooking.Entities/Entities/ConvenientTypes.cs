using HoangNV.HotelBooking.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Entities.Entities
{
    public class ConvenientTypes : BaseEntity
    {
        [Key]
        public int ConvenientTypeId { get; set; }
        [StringLength(255)]
        public string ConvenientTypeName { get; set; }

        public virtual ICollection<Convenients> Convenients { get; set; }
    }
}
