using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class HotelBranchSearchModel
    {
        public string HotelBranchName { get; set; }
        public string HotelBranchCode { get; set; } 
        public string Address { get; set; }
        public int Status { get; set; }
    }
}
