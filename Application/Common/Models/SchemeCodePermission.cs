using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public sealed class SchemeCodePermission
    {
        public int Id { get; set; }
        public PermissionType Permission { get; set; }
        public bool IsPermitted { get; set; }

    }

    public sealed class SchemeCodePermissionResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsPermitted { get; set; }

    }
}
