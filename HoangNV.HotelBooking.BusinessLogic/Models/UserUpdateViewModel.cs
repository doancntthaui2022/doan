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
    public class UserUpdateViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
    public class UserUpdateValidator : AbstractValidator<UserUpdateViewModel>
    {
        public UserUpdateValidator(IUserBS _userBS, IStringLocalizer<UserUpdateViewModel> localizer)
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
            .WithName(localizer["Display_UserName"]);
           
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
        }

        public IEnumerable<UserResultModel> userResults { get; set; }
    }
}
