using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Enum
{
    public enum StatusBookingRoomEnum
    {
        [Display(Name = "Tất cả")]
        All = 0,
        [Display(Name = "Đang xử lý")]
        Booking = 1,
        [Display(Name = "Đã đặt phòng")]
        Booked = 2,
        [Display(Name = "Đã nhận phòng")]
        Book = 3,
        [Display(Name ="Đã trả phòng")]
        Exit =4,
    }
}
