using Application.Common.Models;
using Application.Helper;
using Domain.Entities;
using Domain.Enum;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.Setting
{
    public class AddSettingConfigurationCommand : IRequest<ResponseModel<long>>
    {
        [JsonIgnore]
        public SettingEnum SettingType { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }

    }

    public class AddSettingConfigurationCommandValidator : AbstractValidator<AddSettingConfigurationCommand>
    {
        public AddSettingConfigurationCommandValidator()
        {
            RuleFor(c => c.CountryCode).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
            RuleFor(c => c.SettingType).NotEmpty();
        }
    }

    public class AddSettingConfigurationCommandHandler : IRequestHandler<AddSettingConfigurationCommand, ResponseModel<long>>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;

        public AddSettingConfigurationCommandHandler(ApplicationSettingHelper applicationSettingHelper)
        {
            _applicationSettingHelper = applicationSettingHelper;
        }

        public async Task<ResponseModel<long>> Handle(AddSettingConfigurationCommand request, CancellationToken cancellationToken)
        {
            var setting = new ApplicationSetting { CountryCode = request.CountryCode, SettingType = request.SettingType, Value = request.Value };
            var resp = await _applicationSettingHelper.CreateAppSetting(setting);
            return resp;
        }
    }
}
