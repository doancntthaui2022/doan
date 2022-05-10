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
    public class PassWordUpdateViewModel
    {
        public string UserId { get; set; }
        public string PassWord { get; set; }
        public string PassWordSecond { get; set; }
        public string OldPassWord { get; set; }
    }

    public class PasswordValidator : AbstractValidator<PassWordUpdateViewModel>
    {
        public PasswordValidator(IStringLocalizer<PassWordUpdateViewModel> localizer)
        {

            RuleFor(i => i.PassWord).Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(localizer["E_A002_02"])
               .NotNull()
               .WithMessage(localizer["E_A002_02"])
               .MinimumLength(5)
                .WithMessage(localizer["E_C_0001_01"])
               .MaximumLength(50)
               .WithMessage(localizer["E_C_008_02"])
           .WithName(localizer["Display_Password"]);

            RuleFor(i => i.PassWordSecond).Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(localizer["E_A002_02"])
               .NotNull()
               .WithMessage(localizer["E_A002_02"])
               .MinimumLength(5)
                .WithMessage(localizer["E_C_0001_01"])
               .MaximumLength(50)
               .WithMessage(localizer["E_C_008_02"])
               .Must((model, _) => model.PassWordSecond == model.PassWord).WithMessage(localizer["E_A0001_02"])
           .WithName(localizer["Display_PasswordSecond"]);

            RuleFor(i => i.OldPassWord).Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage(localizer["E_A002_02"])
               .NotNull()
               .WithMessage(localizer["E_A002_02"])
               .MinimumLength(5)
                .WithMessage(localizer["E_C_0001_01"])
               .MaximumLength(50)
               .WithMessage(localizer["E_C_008_02"])
           .WithName(localizer["Display_Password"]);

        }

    }
}
