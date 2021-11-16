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
    public class GetSchemeCodeQuery: IRequest<ResponseModel<SchemeCodeSettingResponse>>
    {
        public long Id { get; set; }
    }

    public class GetSchemeCodeQueryHandler : IRequestHandler<GetSchemeCodeQuery, ResponseModel<SchemeCodeSettingResponse>>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;

        public GetSchemeCodeQueryHandler(ApplicationSettingHelper applicationSettingHelper)
        {
            _applicationSettingHelper = applicationSettingHelper;
        }

        public async Task<ResponseModel<SchemeCodeSettingResponse>> Handle(GetSchemeCodeQuery request, CancellationToken cancellationToken)
        {
            var setting = await _applicationSettingHelper.GetSchemeCode(request.Id);

            var permissions = new List<SchemeCodePermissionResponse>();
            setting.Permissions.ToList().ForEach(p =>
            {
                permissions.Add(new SchemeCodePermissionResponse { Id = p.Id, IsPermitted = p.IsPermitted, Name = p.Permission.GetDescription() });
            });
            return ResponseModel<SchemeCodeSettingResponse>.Success(new SchemeCodeSettingResponse { CountryCode = setting.CountryCode, Id = setting.Id, SettingType = SettingEnum.SchemeCode.GetDescription(), Value = setting.Code, Permissions = permissions });
        }
    }
}
