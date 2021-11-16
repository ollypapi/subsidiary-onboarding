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

    public class InitiateResetPasswordOTPCommand : IRequest<ResponseModel<OtpResponse>>
    {
        public string  PhoneNumber { get; set; }

    }
    public class InitiateResetPasswordOTPCommandValidator : AbstractValidator<InitiateResetPasswordOTPCommand>
    {
        public InitiateResetPasswordOTPCommandValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
        }

    }
    public class InitiateResetPasswordOTPCommandHandler : IRequestHandler<InitiateResetPasswordOTPCommand, ResponseModel<OtpResponse>>
    {
        private readonly VerificationHelper _verificationHelper;
        private readonly CustomerHelper _customerHelper;
        public InitiateResetPasswordOTPCommandHandler(VerificationHelper verificationHelper, CustomerHelper customerHelper)
        {
            _verificationHelper = verificationHelper;
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel<OtpResponse>> Handle(InitiateResetPasswordOTPCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomerFromPhone(request.PhoneNumber);
            if (customer == null)
                throw new CustomException("Unable to get customer...");
           return await _verificationHelper.InitiateOtp(customer.Id,request.PhoneNumber,string.Empty,OtpPurpose.PasswordReset);
        }
    }
}
