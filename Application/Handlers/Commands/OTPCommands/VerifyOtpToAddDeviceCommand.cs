using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Helper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.OTPCommands
{
    public class VerifyOtpToAddDeviceCommand : IRequest<ResponseModel>
    {
        public string Otp { get; set; }
        public string TrackingId { get; set; }
        public string DeviceId { get; set; }
        public string DeviceModel { get; set; }
        public string Os { get; set; }
        public long CustomerId { get; set; }
    }

    public class VerifyOtpToAddDeviceCommandValidator : AbstractValidator<VerifyOtpToAddDeviceCommand>
    {
        public VerifyOtpToAddDeviceCommandValidator()
        {
            RuleFor(c => c.DeviceId).NotEmpty();
            RuleFor(c => c.DeviceModel).NotEmpty();
            RuleFor(c => c.Os).NotEmpty();
            RuleFor(c => c.Otp).NotEmpty();
            RuleFor(c => c.TrackingId).NotEmpty();
        }

        public class InitiateOTPCommandHandler : IRequestHandler<VerifyOtpToAddDeviceCommand, ResponseModel>
        {
            private readonly VerificationHelper _verificationHelper;
            private readonly CustomerHelper _customerHelper;
            public InitiateOTPCommandHandler(VerificationHelper verificationHelper, CustomerHelper customerHelper)
            {
                _verificationHelper = verificationHelper;
                _customerHelper = customerHelper;
            }
            public async Task<ResponseModel> Handle(VerifyOtpToAddDeviceCommand request, CancellationToken cancellationToken)
            {
                var customer = await _customerHelper.GetCustomer(request.CustomerId);

                if(customer == null)
                    throw new CustomException("Customer Not Found");
                var resp = await _verificationHelper.VerifyGeneralOtp(request.TrackingId,request.Otp, Domain.Enum.OtpPurpose.AddDevice);

                if (!resp.Status)
                    throw new CustomException("Invalid OTP");
                var canAddDevice = await _customerHelper.CanAddMoreDevice(request.CustomerId, customer.CountryId);
                var isDeviceExisting = await _customerHelper.isDeviceExisting(request.DeviceId);

                if(isDeviceExisting)
                    throw new CustomException("Sorry, Device already attached to a Profile ");

                if (!canAddDevice)
                    throw new CustomException("Cannot add more device");

                var r = await _customerHelper.CreateDevice(request.CustomerId, new DeviceModel { DeviceId = request.DeviceId, DeviceName = request.DeviceModel, OS = request.Os }, customer.CountryId);

                if(!r)
                    throw new CustomException("Unable to add Device");
                return ResponseModel.Success();

            }
        }

    }
}
