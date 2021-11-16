using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response
{
    public class ApplicationSettingResponse
    {
        public long Id { get; set; }
        public string Value { get; set; }
        public string SettingType { get; set; }
        public string CountryCode { get; set; }

    }

    public class SchemeCodeSettingResponse: ApplicationSettingResponse
    {
        public List<SchemeCodePermissionResponse> Permissions { get; set; }
    }
}
