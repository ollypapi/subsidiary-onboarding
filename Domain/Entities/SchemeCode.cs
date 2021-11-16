using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SchemeCode: BaseEntity
    {
        public string Code { get; set; }
        public string CountryCode { get; set; }

        public ICollection<SchemeCodeSettingPermission> Permissions { get; set; }
    }
}
