using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
using Application.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries.Setting
{
    public class GetSettingConfigurationByIdQuery : IRequest<ResponseModel<ApplicationSettingResponse>>
    {
        public long Id { get; set; }
    }

    public class GetSettingConfigurationByIdQueryHandler : IRequestHandler<GetSettingConfigurationByIdQuery, ResponseModel<ApplicationSettingResponse>>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;

        public GetSettingConfigurationByIdQueryHandler(ApplicationSettingHelper applicationSettingHelper)
        {
            _applicationSettingHelper = applicationSettingHelper;
        }

        public async Task<ResponseModel<ApplicationSettingResponse>> Handle(GetSettingConfigurationByIdQuery request, CancellationToken cancellationToken)
        {
            var setting = await _applicationSettingHelper.GetAppSetting(request.Id);
            return ResponseModel<ApplicationSettingResponse>.Success(new ApplicationSettingResponse { CountryCode = setting.CountryCode, Id = setting.Id, SettingType = setting.SettingType.GetDescription(), Value = setting.Value });
        }
    }

 }
