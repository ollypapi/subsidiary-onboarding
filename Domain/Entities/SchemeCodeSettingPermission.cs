using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SchemeCodeSettingPermission: BaseEntity
    {
        public PermissionType Permission { get; set; }
        public bool IsPermitted { get; set; }
        public long SchemeCodeId { get; set; }
        public  SchemeCode SchemeCode { get; set; }

    }
}
