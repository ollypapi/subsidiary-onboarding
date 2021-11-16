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
   public class ResetPinCommand : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public string TransactionPin { get; set; }
        public string OTP { get; set; }
        public string TrackingId { get; set; }
    }
    public class ResetPinCommandValidator : AbstractValidator<ResetPinCommand>
    {
        public ResetPinCommandValidator()
        {
            RuleFor(c => c.TransactionPin).NotEmpty();
            RuleFor(c => c.OTP).NotEmpty();
            RuleFor(c => c.TrackingId).NotEmpty();
        }   
    }
    public class ResetPinCommandHandler : IRequestHandler<ResetPinCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly VerificationHelper _verificationHelper;
        public ResetPinCommandHandler(CustomerHelper customerHelper, VerificationHelper verificationHelper)
        {
            _customerHelper = customerHelper;
            _verificationHelper = verificationHelper;
        }
        public async Task<ResponseModel> Handle(ResetPinCommand request, CancellationToken cancellationToken)
        {
            var response = await _verificationHelper.VerifyGeneralOtp(request.TrackingId, request.OTP,Domain.Enum.OtpPurpose.PinReset);
            if (!response.Status)
                return response;

            var customer = await _customerHelper.GetCustomer(request.CustomerId);
            if (customer == null)
                throw new CustomException("User not found");

            return await _customerHelper.SetTransactionPin(customer, request.TransactionPin);
        }
    }
}
