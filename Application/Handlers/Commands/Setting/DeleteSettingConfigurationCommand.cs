using Application.Common.Models;
using Application.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.Setting
{
    public class DeleteSettingConfigurationCommand: IRequest<ResponseModel>
    {
        public long Id { get; set; }
        public string CountryCode { get; set; }
    }

    public class DeleteSettingConfigurationCommandHandler : IRequestHandler<DeleteSettingConfigurationCommand, ResponseModel>
    {
        private readonly ApplicationSettingHelper _applicationSettingHelper;

        public DeleteSettingConfigurationCommandHandler(ApplicationSettingHelper applicationSettingHelper)
        {
            _applicationSettingHelper = applicationSettingHelper;
        }

        public async Task<ResponseModel> Handle(DeleteSettingConfigurationCommand request, CancellationToken cancellationToken)
        {
          return  await _applicationSettingHelper.DeleteSetting(request.Id, request.CountryCode);
        }
    }

    }
