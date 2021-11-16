using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Application.Common.Enums
{

    public enum ResponseCodeEnum
    {
        [Description("00")]
        Success = 1,
        [Description("02")]
        ValidationError = 2,
        [Description("03")]
        ApplicationError = 3,
        [Description("04")]
        AuthorizationError = 4,
        [Description("05")]
        UnHandledException = 5,
        [Description("O6")]
        AccountCreationFailed = 6,
        [Description("O7")]
        GetAccountFailed = 7,
        [Description("08")]
        GetNTransactionFailed = 8,
        [Description("09")]
        DeviceValidationFailed = 9,
        [Description("10")]
        AuthenticationFailed = 10,
        [Description("11")]
        DeviceMatchFailed = 11,
        [Description("12")]
         AddDevice = 12,

    }
}
