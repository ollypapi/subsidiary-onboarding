using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
using Application.Helper;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries.Setting
{
    public class GetSettingConfigurationQuery: IRequest<ResponseModel<ApplicationSettingResponse>>
    {
        public SettingEnum SettingType { get; set; }
        public string CountryCode { get; set; }
    }

    public class GetSettingConfigurationQueryHandler : IRequestHandler<GetSettingConfigurationQuery, ResponseModel<ApplicationSettingResponse>>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;

        public GetSettingConfigurationQueryHandler(ApplicationSettingHelper applicationSettingHelper)
        {
            _applicationSettingHelper = applicationSettingHelper;
        }

        public async Task<ResponseModel<ApplicationSettingResponse>> Handle(GetSettingConfigurationQuery request, CancellationToken cancellationToken)
        {
            var setting = await _applicationSettingHelper.GetAppSetting(request.CountryCode,request.SettingType);
            return ResponseModel<ApplicationSettingResponse>.Success(new ApplicationSettingResponse { CountryCode = setting.CountryCode, Id = setting.Id, SettingType = setting.SettingType.GetDescription(), Value = setting.Value });
        }
    }


}
