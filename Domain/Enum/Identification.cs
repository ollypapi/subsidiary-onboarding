using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Enum
{
    public enum Identification
    {
        [Description("Driver's License")]
        Driver_License = 1,
        [Description("International Passport")]
        International_Passport
    }
    public enum DocumentType
    {
        [Description("Selfie")]
        Selfie = 1,
        [Description("Signature")]
        Signature,
        [Description("Identity")]
        Identity,
        [Description("CertificateOfResidence")]
        CertificateOfResidence

    }

    public enum PermissionType
    {
        View=1,
        Debit,
        Credit
    }
    
    public enum DocumentStatus
    {
        Pending = 1,
        Rejected,
        Approved
    }

    public enum CustomerStatus
    {
        ACTIVATED = 1,
        DEACTIVATED,
    }

    public enum DeviceStatus
    {
        Released = 1,
        Deactivated,
        Activated
    }

    public enum OtpPurpose
    {
        Onboarding,
        PinReset,
        PasswordReset,
        SecurityQuestion,
        AddDevice
    }
    public enum OtpStatus
    {
        Initiated,
        Applied,
        Expired
    }
    public enum RegistrationStage
    {
        ProfileCreated,
        OtpInititated,
        OtpVerified,
        PasswordSet,
        PinSet,
        SecurityQuestionSet,
        DocumentUploadCompleted,
        AccountCreated,
        OnboardingCompleted
    }
}
