using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
using Application.Helper;
using Domain.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries.Setting
{
    public class GetSchemeCodesQuery: IRequest<ResponseModel<List<SchemeCodeSettingResponse>>>
    {
        public string CountryCode { get; set; }
        public SettingEnum SettingType { get; set; }
    }

    public class GetSchemeCodesQueryHandler : IRequestHandler<GetSchemeCodesQuery, ResponseModel< List<SchemeCodeSettingResponse>>>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;

        public GetSchemeCodesQueryHandler(ApplicationSettingHelper applicationSettingHelper)
        {
            _applicationSettingHelper = applicationSettingHelper;
        }

        public async Task<ResponseModel<List<SchemeCodeSettingResponse>>> Handle(GetSchemeCodesQuery request, CancellationToken cancellationToken)
        {
            var settings = await _applicationSettingHelper.GetSchemeCodes(request.CountryCode);
            var SchemeCodeSettings = new List<SchemeCodeSettingResponse>();
            settings.ForEach( async setting => {
                //var settingPermissions = await _applicationSettingHelper.GetSchemeCodePermissions(setting.Id);
                var permissions = new List<SchemeCodePermissionResponse>();
                setting.Permissions.ToList().ForEach(p =>
                {
                    permissions.Add(new SchemeCodePermissionResponse { Id = p.Id, IsPermitted = p.IsPermitted, Name = p.Permission.GetDescription() });
                });
                SchemeCodeSettings.Add(new SchemeCodeSettingResponse { CountryCode = setting.CountryCode, Id = setting.Id, SettingType = SettingEnum.SchemeCode.GetDescription(), Value = setting.Code, Permissions = permissions });
            });
            return ResponseModel< List<SchemeCodeSettingResponse>>.Success(SchemeCodeSettings);
        }
    }
}
