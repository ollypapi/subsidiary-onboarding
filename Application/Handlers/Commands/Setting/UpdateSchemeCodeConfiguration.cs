using Application.Common.Models;
using Application.Helper;
using Application.Interfaces;
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

    public class UpdateSchemeCodeConfiguration : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public SettingEnum SettingType { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }
        public long Id { get; set; }
        public List<SchemeCodePermission> Permissions { get; set; }

    }

    public class UpdateSchemeCodeConfigurationValidator : AbstractValidator<UpdateSchemeCodeConfiguration>
    {
        public UpdateSchemeCodeConfigurationValidator()
        {
            RuleFor(c => c.CountryCode).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
            RuleFor(c => c.SettingType).IsInEnum();
            RuleFor(c => c.Id).GreaterThan(0);

        }
    }

    public class UpdateSchemeCodeConfigurationHandler : IRequestHandler<UpdateSchemeCodeConfiguration, ResponseModel>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;
        private readonly ISchemeCodeService _schemeCodeService;

        public UpdateSchemeCodeConfigurationHandler(ApplicationSettingHelper applicationSettingHelper, ISchemeCodeService schemeCodeService)
        {
            _applicationSettingHelper = applicationSettingHelper;
            _schemeCodeService = schemeCodeService;
        }

        public async Task<ResponseModel> Handle(UpdateSchemeCodeConfiguration request, CancellationToken cancellationToken)
        {
            var setting = await _applicationSettingHelper.GetSchemeCode(request.Id);
            setting.Code = request.Value != null || request.Value != "" ? request.Value : setting.Code;
            setting.Permissions.Clear();
            request.Permissions.ForEach(p => {
                setting.Permissions.Add(new SchemeCodeSettingPermission { Id = p.Id, IsPermitted = p.IsPermitted, Permission = p.Permission, SchemeCodeId = setting.Id });
            });
            var resp = await _applicationSettingHelper.UpdateSchemeCode(setting);

            if (resp.Status)
            {
                var data = new Common.Models.AccountService.SchemeCode { Code = request.Value, CountryCode = request.CountryCode,Id=request.Id, Permissions = new List<Common.Models.AccountService.SchemeCodePermission>() };
                request.Permissions.ForEach(p => {
                    data.Permissions.Add(new Common.Models.AccountService.SchemeCodePermission { IsPermitted = p.IsPermitted, Permission = p.Permission,Id=p.Id });
                });
                var r = await _schemeCodeService.AddSchemeCode(data);
            }
            return resp;
        }
    }


}
