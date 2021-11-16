using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Helper;
using Domain.Entities;
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
   public class ResetPasswordCommand : IRequest<ResponseModel>
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
        public string ResetPasswordToken { get; set; }
    }
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(c => c.Password).NotEmpty();
            RuleFor(c => c.OTP).NotEmpty();
            RuleFor(c => c.ResetPasswordToken).NotEmpty();
        }   
    }
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly VerificationHelper _verificationHelper;
        public ResetPasswordCommandHandler(CustomerHelper customerHelper, VerificationHelper verificationHelper)
        {
            _customerHelper = customerHelper;
            _verificationHelper = verificationHelper;
        }
        public async Task<ResponseModel> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var response = await _verificationHelper.VerifyGeneralOtp(request.ResetPasswordToken, request.OTP,Domain.Enum.OtpPurpose.PasswordReset);
            if (!response.Status)
                return response;

            var customer = await _customerHelper.GetCustomerFromPhone(request.PhoneNumber);
            if (customer == null)
                throw new CustomException("User not found");

            return await _customerHelper.SetPassword(customer, request.Password);
        }
    }
}
