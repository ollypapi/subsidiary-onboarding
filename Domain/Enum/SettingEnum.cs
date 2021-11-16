using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Enum
{
    public enum SettingEnum
    {
        PasswordRetryMaxCount=1,
        MaxSecurityQuestionCount,
        MaxDeviceCount,
        MaxConcurrentLogin,
        SchemeCode,
        TransactionHistoryCount
    }

    public enum SchemeCodeRestritionEnum
    {
        NoTransferCharge,
        NoCredit,
        NoDebit,
        NoView
    }

    public enum DeviceHistoryEnum
    {
        Device_Created=1,
        Device_Enabled,
        Device_Disabled
    }
}
