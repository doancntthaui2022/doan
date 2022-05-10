using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Enum
{
    public enum HotelBranchStatus
    {
        [Display(Name="Tất cả")]
        All=0,
        [Display(Name ="Đang hoạt động")]
        Active=1,
        [Display(Name ="Tạm thời ngừng hoạt động")]
        NonActive=2,
        [Display(Name ="Ngừng hoạt động")]
        NonActived=3,
    }
}
