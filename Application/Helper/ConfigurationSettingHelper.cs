using Application.Common.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Helper
{
    public sealed class ConfigurationSettingHelper
    {
        private readonly IConfiguration _configuration;
        public ConfigurationSettingHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AppSettingModel AppSetting()
        {
            // var charge = _configuration.GetValue<ChargeModel>($"AppSettings:ServiceCharge:BankTransfer:KE:IntraBank:{Currency.ToUpper()}:Commission");
            var charge = _configuration.GetSection("").Get<AppSettingModel>();
            return charge;
        }

       
    }
}
