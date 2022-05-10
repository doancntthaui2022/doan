using FluentValidation;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class RoomTypeSearchModel
    {
        public string RoomTypeCode { get; set; }
        public string RoomTypeName { get; set; }
    }
    public class RoomTypeViewModel
    {
        public int? RoomTypeId { get; set; }
        public string RoomTypeCode { get; set; }
        public string RoomTypeName { get; set; }
        public decimal? Cost { get; set; }
        public string Description { get; set; }
        public float? Area { get; set; }
        public int? NumOfPer { get; set; }
        public int? HotelBranchId { get; set; }

        public List<int> ConvenientId { get; set; } = new List<int>();
        public List<string> BedNumber { get; set; } = new List<string>();
        public List<string> ConvenientName { get; set; } = new List<string>();
        public int? NumOfRoom { get; set; }
        public List<string> ImageLinks { get; set; } = new List<string>();
    }

    public class RoomTypeValidator : AbstractValidator<RoomTypeViewModel>
    {
        public RoomTypeValidator(IRoomTypeBS _roomTypeBS, IStringLocalizer<RoomTypeViewModel> localizer)
        {
            RoomTypeModels = _roomTypeBS.Search(new RoomTypeSearchModel() { RoomTypeCode = string.Empty, RoomTypeName = string.Empty }).Result;
            RuleFor(x => x.RoomTypeCode).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
                .Must((model, _) => !RoomTypeModels.Any(j => j.RoomTypeCode.Trim().ToLower() == model.RoomTypeCode.Trim().ToLower()))
                .WithMessage(localizer["E_A_004_01"])
            .WithName(localizer["Display_RoomTypeCode"]);

            RuleFor(x => x.RoomTypeName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
            .WithName(localizer["Display_RoomTypeName"]);

            RuleFor(x => x.NumOfPer).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .GreaterThanOrEqualTo(0)
                .WithMessage(localizer["E_C_008_01"])
            .WithName(localizer["Display_RoomTypeNumOfPer"]);

            RuleFor(x => x.Area).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .GreaterThanOrEqualTo(0)
                .WithMessage(localizer["E_C_008_01"])
            .WithName(localizer["Display_RoomTypeArea"]);

            RuleFor(x => x.Cost).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .GreaterThanOrEqualTo(0)
                .WithMessage(localizer["E_C_008_01"])
            .WithName(localizer["Display_RoomTypeCost"]);

        }

        public IEnumerable<RoomTypeViewModel> RoomTypeModels { get; set; }

    }
}
