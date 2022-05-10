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
    public class UserAddViewModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string PassWordSecond { get; set; }
        public int RoleId { get; set; }
    }

    public class UserAddValidator : AbstractValidator<UserAddViewModel>
    {
        public UserAddValidator(IUserBS _userBS, IStringLocalizer<UserAddViewModel> localizer)
        {
            userResults = _userBS.SearchAll().Result;
            RuleFor(i => i.UserName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MinimumLength(5)
                .WithMessage(localizer["E_C_0001_01"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
                .Must((model, _) => !userResults.Any(j => j.UserName.Trim().ToLower() == model.UserName.Trim().ToLower()))
                .WithMessage(localizer["E_A_004_01"])
            .WithName(localizer["Display_UserName"]);

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
               .Must((model, _) => model.PassWordSecond==model.PassWord).WithMessage(localizer["E_A0001_02"])
           .WithName(localizer["Display_PasswordSecond"]);


            RuleFor(i => i.Email).Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage(localizer["E_A002_02"])
              .NotNull()
              .WithMessage(localizer["E_A002_02"])
              .MaximumLength(50)
              .WithMessage(localizer["E_C_008_02"])
              .EmailAddress()
              .WithMessage(localizer["E_C_001_09"])
          .WithName(localizer["Display_Email"]);

            RuleFor(i => i.FullName).Cascade(CascadeMode.Stop)
              .NotEmpty().WithMessage(localizer["E_A002_02"])
              .NotNull()
              .WithMessage(localizer["E_A002_02"])
              .MaximumLength(50)
              .WithMessage(localizer["E_C_008_02"])
          .WithName(localizer["Display_FullName"]);

            RuleFor(i => i.RoleId).Cascade(CascadeMode.Stop)
              .NotEqual(0)
              .WithMessage(localizer["E_A002_02"])

          .WithName(localizer["Display_Role"]);
        }

        public IEnumerable<UserResultModel> userResults { get; set; }
    }
}
