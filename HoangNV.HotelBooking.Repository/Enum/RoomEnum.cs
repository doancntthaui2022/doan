using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.Repository.Enum
{
    public enum RoomEnum
    {
        [Display(Name = "Tất cả")]
        All =0,
        [Display(Name = "Đang hoạt động")]
        Free =1,
        [Display(Name = "Không hoạt động")]
        Nonactive =4,
    }
}
