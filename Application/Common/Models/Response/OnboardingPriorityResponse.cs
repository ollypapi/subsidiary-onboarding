using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response
{
    public class OnboardingPriorityResponse
    {
        public string OnboardingPriority { get; set; }
        public Token Token { get; set; }
    }
    public enum RegistrationStageEnum
    {
        ProfileCreated,
        OtpInititated,
        OtpVerified,
        PasswordSet,
        PinSet,
        DocumentUploadCompleted,
        AccountCreated,
        OnboardingCompleted
    }
}
