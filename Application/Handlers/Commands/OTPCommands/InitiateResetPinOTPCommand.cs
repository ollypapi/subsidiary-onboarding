using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Helper;
using Domain.Entities;
using Domain.Enum;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.OTPCommands
{

    public class InitiateResetPinOTPCommand : IRequest<ResponseModel<OtpResponse>>
    {
        public string  PhoneNumber { get; set; }

    }
    public class InitiateResetPinOTPCommandValidator : AbstractValidator<InitiateResetPinOTPCommand>
    {
        public InitiateResetPinOTPCommandValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
        }

    }
    public class InitiateResetPinOTPCommandValidatorHandler : IRequestHandler<InitiateResetPinOTPCommand, ResponseModel<OtpResponse>>
    {
        private readonly VerificationHelper _verificationHelper;
        private readonly CustomerHelper _customerHelper;
        public InitiateResetPinOTPCommandValidatorHandler(VerificationHelper verificationHelper, CustomerHelper customerHelper)
        {
            _verificationHelper = verificationHelper;
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel<OtpResponse>> Handle(InitiateResetPinOTPCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomerFromPhone(request.PhoneNumber);
            if (customer == null)
                throw new CustomException("Unable to get customer...");
           return await _verificationHelper.InitiateOtp(customer.Id,request.PhoneNumber,string.Empty,OtpPurpose.PinReset);
        }
    }
}
