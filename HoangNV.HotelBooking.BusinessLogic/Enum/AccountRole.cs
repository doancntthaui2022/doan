using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Enum
{
    public enum AccountRole
    {
        [Display(Name = "Tất cả")]
        All = 0,
        [Display(Name = "Nhân viên")]
        Employee = 2,
        [Display(Name = "Khách hàng")]
        Client = 3,
    }

    public enum AccountRole2
    {
        All = 0,
        Admin = 1,
        Employee = 2,
        Client = 3,
    }
}
