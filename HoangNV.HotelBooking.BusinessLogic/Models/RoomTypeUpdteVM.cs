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
    public class RoomTypeUpdteVM
    {
        public int RoomTypeId { get; set; }
        public string RoomTypeCode { get; set; }
        public string RoomTypeName { get; set; }
        public decimal? Cost { get; set; }
        public string Description { get; set; }
        public float? Area { get; set; }
        public int? NumOfPer { get; set; }
        public int? HotelBranchId { get; set; }

        public List<int> ConvenientId { get; set; } = new List<int>();
        public List<string> BedNumber { get; set; } = new List<string>();
        public int? NumOfRoom { get; set; }
        public List<string> ImageLinks { get; set; } = new List<string>();
    }

    public class RoomTypeUpdateValidator : AbstractValidator<RoomTypeUpdteVM>
    {
        public RoomTypeUpdateValidator(IRoomTypeBS _roomTypeBS, IStringLocalizer<RoomTypeViewModel> localizer)
        {
            var roomTypes = _roomTypeBS.Search(new RoomTypeSearchModel() { RoomTypeCode = string.Empty, RoomTypeName = string.Empty }).Result;
            RoomTypeModels = roomTypes.Select(x =>
            new RoomTypeUpdteVM(){
                RoomTypeId= (int)x.RoomTypeId,
                RoomTypeCode= x.RoomTypeCode,
                RoomTypeName=x.RoomTypeName,
                Area=x.Area,
                BedNumber=x.BedNumber,
                ConvenientId=x.ConvenientId,
                Cost=x.Cost,
                Description=x.Description,
                HotelBranchId=x.HotelBranchId,
                ImageLinks=x.ImageLinks,
                NumOfPer=x.NumOfPer,
                NumOfRoom=x.NumOfRoom,
            }).ToList();
            RuleFor(x => x.RoomTypeCode).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
                .Must((model, _) => !RoomTypeModels.Any(j => j.RoomTypeCode.Trim().ToLower() == model.RoomTypeCode.Trim().ToLower() && j.RoomTypeId!=model.RoomTypeId))
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

        public IEnumerable<RoomTypeUpdteVM> RoomTypeModels { get; set; }
    }
}
