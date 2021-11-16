using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Application.Common.Models.AccountService
{
    public class SchemeCode
    {
        public long Id { get; set; }
        public string Code { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }
        public List<SchemeCodePermission> Permissions { get; set; }
    }

    public class SchemeCodePermission
    {
        public long Id { get; set; }
        public PermissionType Permission { get; set; }
        public bool IsPermitted { get; set; }

    }
}
