using FluentValidation;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.Repository.Models;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class BedViewModel
    {
        public int BedId { get; set; }
        public string BedType { get; set; }
    }

    public class BedValidator : AbstractValidator<BedViewModel>
    {
        public BedValidator(IBedBS _bedBS, IStringLocalizer<BedViewModel> localizer)
        {
            BedQueryModels = _bedBS.Search(string.Empty).Result;

            RuleFor(i => i.BedType).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
                .Must((model, _) => !BedQueryModels.Any(j => j.BedType.Trim().ToLower() == model.BedType.Trim().ToLower() && j.BedId != model.BedId))
                .WithMessage(localizer["E_A_004_01"])
            .WithName(localizer["Display_BedsName"]);
        }

        public IEnumerable<BedQueryModel> BedQueryModels { get; set; }
    }
}
