using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ApplicationSetting: BaseEntity
    {
        public SettingEnum SettingType { get; set; }
        public string Value { get; set; }
        public string CountryCode { get; set; }


    }
}
