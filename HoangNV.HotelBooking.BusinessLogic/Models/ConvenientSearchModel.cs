using FluentValidation;
using HoangNV.HotelBooking.BusinessLogic.Interface;
using HoangNV.HotelBooking.Repository.Models;
using Microsoft.Extensions.Localization;
using Mts.Mes.HealthSupport.BusinessLogic.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangNV.HotelBooking.BusinessLogic.Models
{
    public class ConvenientSearchModel
    {
        public string ConvenientName {get; set; }
        public int ConvenientTypeId { get; set; }
    }
    public class ConvenientViewModel
    {
        public int ConvenientId { get; set; }
        public string ConvenientName { get; set; }
        public int ConvenientTypeId { get; set; }
    }
    public class ConvenientValidator: AbstractValidator<ConvenientViewModel>
    {
        public ConvenientValidator(IConvenientBS _convenientBS, IStringLocalizer<ConvenientViewModel> localizer)
        {
            ConvenientQueryModels = _convenientBS.Search(new ConvenientSearchModel()).Result;

            RuleFor(i => i.ConvenientName).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer["E_A002_02"])
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .MaximumLength(50)
                .WithMessage(localizer["E_C_008_02"])
                .Must((model, _) => !ConvenientQueryModels.Any(j => j.ConvenientName.Trim().ToLower() == model.ConvenientName.Trim().ToLower() && j.ConvenientId != model.ConvenientId))
                .WithMessage(localizer["E_A_004_01"])
            .WithName(localizer["Display_ConvenientName"]);

            RuleFor(i => i.ConvenientTypeId).Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(localizer["E_A002_02"])
                .Must((model, _) => !(model.ConvenientTypeId == 0))
                .WithMessage(localizer["E_A002_02"])
            .WithName(localizer["Display_ConvenientTypeName"]);
        }

        public IEnumerable<ConvenientQueryModel> ConvenientQueryModels { get; set; }
    }
}
