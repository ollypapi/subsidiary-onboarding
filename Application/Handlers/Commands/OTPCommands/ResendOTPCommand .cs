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

    public class ResendOTPCommand : IRequest<ResponseModel<OtpResponse>>
    {
        public string  PhoneNumber { get; set; }
        public string TrackingNumber { get; set; }
        public OtpPurpose otpPurpose { get; set; }

    }
    public class ResendOTPCommandValidator: AbstractValidator<ResendOTPCommand>
    {
        public ResendOTPCommandValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.TrackingNumber).NotEmpty();
        }

    }
    public class ResendOTPCommandHandler : IRequestHandler<ResendOTPCommand, ResponseModel<OtpResponse>>
    {
        private readonly VerificationHelper _verificationHelper;
        private readonly CustomerHelper _customerHelper;
        public ResendOTPCommandHandler(VerificationHelper verificationHelper, CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
            _verificationHelper = verificationHelper;
        }
        public async Task<ResponseModel<OtpResponse>> Handle(ResendOTPCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomerFromPhone(request.PhoneNumber);
            if (customer == null)
                throw new CustomException("Unable to get customer...");
            return await _verificationHelper.InitiateOtp(customer.Id,request.PhoneNumber, request.TrackingNumber,request.otpPurpose);
        }
    }
}
