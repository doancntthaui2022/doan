using System;
using System.Collections.Generic;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class BookingDetailViewModel
    {
        public List<int> RoomTypeId { get; set; }
        public List<int> NumOfRoom { get; set; }
        public List<decimal> CostTotal { get; set; }
        public decimal? CostSum { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime CheckOutTime { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CheckInPersonCode { get; set; }
        public string CheckInPersonName { get; set; }
    }
    public class BookingDetailValidator : AbstractValidator<BookingDetailViewModel>
    {
        public BookingDetailValidator()
        {

            RuleFor(i => i.CustomerName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tên người đặt phòng không được để trống")
                .NotNull()
                .WithMessage("Tên người đặt phòng không được để trống")
                .MaximumLength(50)
                .WithMessage("Tên người đặt phòng không được vượt quá 50 ký tự")
            .WithName("Tên người đặt phòng");

            RuleFor(i => i.PhoneNumber).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Số điện thoại không được để trống")
                .NotNull()
                .WithMessage("Số điện thoại không được để trống")
                .MaximumLength(50)
                .WithMessage("Số điện thoại không được vượt quá 50 ký tự")
            .WithName("Số điện thoại");

            RuleFor(i => i.Email).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email không được để trống")
                .NotNull()
                .WithMessage("Email không được để trống")
                .MaximumLength(50)
                .WithMessage("Email không được vượt quá 50 ký tự")
                .EmailAddress()
                .WithMessage("Email sai định dạng")
            .WithName("Email");

            RuleFor(i => i.CheckInPersonCode).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("CNMD người nhận phòng không được để trống")
                .NotNull()
                .WithMessage("CNMD người nhận phòng không được để trống")
                .MaximumLength(50)
                .WithMessage("CNMD người nhận phòng không được vượt quá 50 ký tự")
            .WithName("CNMD người nhận phòng");
            RuleFor(i => i.CheckInPersonName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Tên người nhận phòng không được để trống")
                .NotNull()
                .WithMessage("Tên người nhận phòng không được để trống")
                .MaximumLength(50)
                .WithMessage("Tên người nhận phòng không được vượt quá 50 ký tự")
            .WithName("Tên người nhận phòng");

        }
    }
}
