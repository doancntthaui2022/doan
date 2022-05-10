using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Models
{
    public class HotelBranchQueryModel
    {
        public int HotelBranchId { get; set; }
        public string HotelBranchName { get; set; }
        public string HotelBranchCode { get; set; }
        public string Address { get; set; }
        public int? Status { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> ImageLinks { get; set; }
    }
}
