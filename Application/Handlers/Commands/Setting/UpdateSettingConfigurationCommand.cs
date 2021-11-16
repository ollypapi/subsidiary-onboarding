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

    public class UpdateSettingConfigurationCommand : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public SettingEnum SettingType { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }
        public long Id { get; set; }
      

    }

    public class UpdateSettingConfigurationCommandValidator : AbstractValidator<UpdateSettingConfigurationCommand>
    {
        public UpdateSettingConfigurationCommandValidator()
        {
            RuleFor(c => c.CountryCode).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
            RuleFor(c => c.SettingType).IsInEnum();
            RuleFor(c => c.Id).GreaterThan(0);
           // RuleFor(c => c.Permissions).NotNull();
        }
    }

    public class UpdateSettingConfigurationCommandHandler : IRequestHandler<UpdateSettingConfigurationCommand, ResponseModel>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;

        public UpdateSettingConfigurationCommandHandler(ApplicationSettingHelper applicationSettingHelper)
        {
            _applicationSettingHelper = applicationSettingHelper;
        }

        public async Task<ResponseModel> Handle(UpdateSettingConfigurationCommand request, CancellationToken cancellationToken)
        {
            var setting = await _applicationSettingHelper.GetAppSetting(request.Id);
            setting.Value = request.Value != null || request.Value != "" ? request.Value : setting.Value;
            var resp = await _applicationSettingHelper.UpdateSetting(setting);
            return resp;
        }
    }

   
}
