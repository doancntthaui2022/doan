using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Models
{
    public class ConvenientQueryModel
    {
        public int ConvenientId { get; set; }
        public int ConvenientTypeId { get; set; }
        public string ConvenientName { get; set; }
        public string ConvenientTypeName { get; set; }
    }
}
