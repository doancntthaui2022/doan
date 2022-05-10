using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class RoomViewModel
    {
        public int RoomId { get; set; }
        public string RoomCode { get; set; }
        public int RoomTypeId { get; set; }
        public int Status { get; set; }
    }

    public class RoomAddViewModel
    {
        public string RoomCode { get; set; }
        public int RoomTypeId { get; set; }
    }

    public class RoomAddValidator : AbstractValidator<RoomAddViewModel>
    {
        public RoomAddValidator( IStringLocalizer<RoomAddViewModel> localizer)
        {
            RuleFor(i => i.RoomTypeId).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
            .WithName(localizer["Display_RoomTypeName"]);
        }

    }
}
