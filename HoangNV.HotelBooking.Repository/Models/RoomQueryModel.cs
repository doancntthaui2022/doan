using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Models
{
    public class RoomQueryModel
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public int RoomTypeId { get; set; }
        public int Status { get; set; }
    }
}
