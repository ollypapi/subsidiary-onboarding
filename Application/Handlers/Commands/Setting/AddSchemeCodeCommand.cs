using Application.Common.Models;
using Application.Extentions;
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
    public class AddSchemeCodeCommand: IRequest<ResponseModel<long>>
    {
        [JsonIgnore]
        public SettingEnum SettingType { get; set; }
        public string Value { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }
        public List<SchemeCodePermission> Permissions { get; set; }
    }

    public class AddSchemeCodeCommandValidator : AbstractValidator<AddSchemeCodeCommand>
    {
        public AddSchemeCodeCommandValidator()
        {
            RuleFor(c => c.CountryCode).NotEmpty();
            RuleFor(c => c.Value).NotEmpty();
            RuleFor(c => c.SettingType).IsInEnum();

        }
    }

    public class AddSchemeCodeCommandHandler : IRequestHandler<AddSchemeCodeCommand, ResponseModel<long>>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;
        private readonly ISchemeCodeService _schemeCodeService;

        public AddSchemeCodeCommandHandler(ApplicationSettingHelper applicationSettingHelper, ISchemeCodeService schemeCodeService)
        {
            _applicationSettingHelper = applicationSettingHelper;
            _schemeCodeService = schemeCodeService;

        }

        public async Task<ResponseModel<long>> Handle(AddSchemeCodeCommand request, CancellationToken cancellationToken)
        {
            var setting = new SchemeCode { CountryCode = request.CountryCode, Code = request.Value,Permissions = new List<SchemeCodeSettingPermission>() };
            request.Permissions.ForEach(p => {
                setting.Permissions.Add(new SchemeCodeSettingPermission { IsPermitted = p.IsPermitted, Permission = p.Permission });
            });
            var resp = await _applicationSettingHelper.CreateSchemeCodeAppSetting(setting);

            if (resp.Status)
            {
                var data = new Common.Models.AccountService.SchemeCode { Code = request.Value, CountryCode = request.CountryCode,Permissions = new List<Common.Models.AccountService.SchemeCodePermission>() };
                request.Permissions.ForEach(p => {
                    data.Permissions.Add( new Common.Models.AccountService.SchemeCodePermission { IsPermitted= p.IsPermitted,Permission=p.Permission});
                });
              var r =  await _schemeCodeService.AddSchemeCode(data);
            }
     
            return resp;
        }
    }


}
