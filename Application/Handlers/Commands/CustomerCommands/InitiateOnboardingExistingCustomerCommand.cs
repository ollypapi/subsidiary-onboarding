using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Helper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
    public class InitiateOnboardingExistingCustomerCommand : IRequest<ResponseModel<OtpResponse>>
    {
        public string MobileNumber { get; set; }
        public string AccountNumber { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }

    }

    public class InitiateOnboardingExistingCustomerCommandValidator : AbstractValidator<InitiateOnboardingExistingCustomerCommand>
    {
        public InitiateOnboardingExistingCustomerCommandValidator()
        {
            RuleFor(o => o.AccountNumber).NotNull();
            RuleFor(o => o.MobileNumber).NotNull();
            RuleFor(o => o.CountryCode).NotNull();
        }
    }


    public class InitiateOnboardingExistingCustomerCommandHandler : IRequestHandler<InitiateOnboardingExistingCustomerCommand, ResponseModel<OtpResponse>>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly VerificationHelper _verificationHelper;

        public InitiateOnboardingExistingCustomerCommandHandler(CustomerHelper customerHelper, VerificationHelper verificationHelper)
        {
            _customerHelper = customerHelper;
            _verificationHelper = verificationHelper;
        }

        public async Task<ResponseModel<OtpResponse>> Handle(InitiateOnboardingExistingCustomerCommand request, CancellationToken cancellationToken)
        {
            var resp = await _customerHelper.OnboardExistingCustomer(request.MobileNumber, request.AccountNumber, request.CountryCode);
            return  await _verificationHelper.InitiateOtp(0, resp.Data.AccountNumber, null, Domain.Enum.OtpPurpose.Onboarding,resp.Data.Email);
            
        }
    }


}
