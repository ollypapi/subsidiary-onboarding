using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Extentions;
using Application.Helper;
using Domain.Entities;
using Domain.Enum;
using FluentValidation;
using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
    public class OnboardExistingCustomerCommand : IRequest<ResponseModel>
    {
        public string MobileNumber { get; set; }
        public string AccountNumber { get; set; }
        [JsonIgnore]
        public string CountryCode { get; set; }
        public DeviceModel DeviceModel { get; set; }
        public string OTP { get; set; }
        public string TrackingId { get; set; }
    }

    public class OnboardExistingCustomerCommandValidator : AbstractValidator<OnboardExistingCustomerCommand>
    {
        public OnboardExistingCustomerCommandValidator()
        {
            RuleFor(o => o.AccountNumber).NotNull();
            RuleFor(o => o.MobileNumber).NotNull();
            RuleFor(o => o.CountryCode).NotNull();
        }
    }
    public class OnboardExistingCustomerCommandHandler : IRequestHandler<OnboardExistingCustomerCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly VerificationHelper _verificationHelper;

        public OnboardExistingCustomerCommandHandler(CustomerHelper customerHelper, VerificationHelper verificationHelper)
        {
            _customerHelper = customerHelper;
            _verificationHelper = verificationHelper;
        }
        public async Task<ResponseModel> Handle(OnboardExistingCustomerCommand request, CancellationToken cancellationToken)
        {
            if(request.OTP != null) 
                await _verificationHelper.VerifyGeneralOtp(request.TrackingId, request.OTP);

            var resp = await _customerHelper.OnboardExistingCustomer(request.MobileNumber, request.AccountNumber, request.CountryCode);
            // check if the customer is an existing customer that has not been profiled
            if (resp.Status && resp.Data.Stage == null)
            {
                var isDeviceExisting = await _customerHelper.isDeviceExisting(request.DeviceModel.DeviceId);
                if (isDeviceExisting)
                    throw new CustomException("Device mismatch");

                var customer = new Customer
                {
                    DateCreated = DateTime.Now,
                    DOB = resp.Data.DateOfBirth,
                    Email = resp.Data.Email,
                    DeviceId = request.DeviceModel.DeviceId,
                    FirstName = resp.Data.FirstName,
                    LastName = resp.Data.LastName,
                    Gender = resp.Data.Gender,
                    PhoneNumber = resp.Data.MobileNo.formatPhoneNumber(),
                    MiddleName = resp.Data.MiddleName,
                    Stage = Domain.Enum.RegistrationStage.DocumentUploadCompleted,
                    Address = resp.Data.Address,
                    State = resp.Data.State,
                    Title = resp.Data.Title,
                    Nationality = resp.Data.Country,
                    CountryId = request.CountryCode,
                    Status = Domain.Enum.CustomerStatus.ACTIVATED.GetDescription(),
                    SubsidiaryId = request.CountryCode,
                    CifId = resp.Data.CifId,
                    CustomerId = resp.Data.CustomerId,
                    AccountNumber = request.AccountNumber,
                    IsExistingCustomer = true
                };

                var profileResponse = await _customerHelper.CreateCustomer(customer);
                if (!profileResponse.Status)
                    throw new CustomException("Attempt to create customer profile failed");
                var deviceCreationResponse = await _customerHelper.CreateDevice(customer.Id, request.DeviceModel, request.CountryCode);
                if (!deviceCreationResponse)
                    throw new CustomException("Weird!. Device failed after profile has been successfully created");
                resp.Data.Stage = Domain.Enum.RegistrationStage.ProfileCreated.GetDescription();
            }
            else
            {
                // A check to know if a customer attempt to continue onboarding with a new device
                if (!await _customerHelper.VerifyCustomerDevice(request.AccountNumber, request.DeviceModel.DeviceId))
                    throw new CustomException("Customer already Exist with another device");
            }
            return resp;
        }
           
    }
}
