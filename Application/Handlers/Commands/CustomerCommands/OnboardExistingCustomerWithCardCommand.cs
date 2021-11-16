using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Requests.FIBServiceRequests;
using Application.Extentions;
using Application.Helper;
using Domain.Entities;
using Domain.Enum;
using FluentValidation;
using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
    public class OnboardExistingCustomerWithCardCommand : IRequest<ResponseModel>
    {
        public string MobileNumber { get; set; }
        public string AccountNumber { get; set; }
        public string CarPan { get; set; }
        public string Pin { get; set; }

        [JsonIgnore]
        public string CountryCode { get; set; }
        public DeviceModel DeviceModel { get; set; }
    }

    public class OnboardExistingCustomerWithCardCommandValidator : AbstractValidator<OnboardExistingCustomerWithCardCommand>
    {
        public OnboardExistingCustomerWithCardCommandValidator()
        {
            RuleFor(o => o.AccountNumber).NotEmpty();
            RuleFor(o => o.MobileNumber).NotEmpty();
            RuleFor(o => o.CountryCode).NotEmpty();
            RuleFor(o => o.CarPan).NotEmpty();
            RuleFor(o => o.Pin).NotEmpty().MaximumLength(4);
        }
    }
    public class OnboardExistingCustomerWithCardCommandHandler : IRequestHandler<OnboardExistingCustomerWithCardCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;

        public OnboardExistingCustomerWithCardCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(OnboardExistingCustomerWithCardCommand request, CancellationToken cancellationToken)
        {
            var isDeviceExisting = await _customerHelper.isDeviceExisting(request.DeviceModel.DeviceId);
            if (isDeviceExisting)
                throw new CustomException("Device mismatch");

            var resp = await _customerHelper.OnboardExistingCustomerWithCard(new CustomerCardValidationRequest 
            {
                AccountNumber = request.AccountNumber,
                MobileNumber = request.MobileNumber,
                CountryId = request.CountryCode,
                CardPan = request.CarPan,
                Pin = request.Pin
            });

            if (resp.Status && resp.Data.Stage == null)
            {
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
                    Stage = RegistrationStage.ProfileCreated,
                    Address = resp.Data.Address,
                    State = resp.Data.State,
                    Title = resp.Data.Title,
                    Nationality = resp.Data.Country,
                    CountryId = request.CountryCode,
                    Status = CustomerStatus.ACTIVATED.GetDescription(),
                    SubsidiaryId = request.CountryCode,
                    CifId = resp.Data.CifId,
                    CustomerId = resp.Data.CustomerId,
                    AccountNumber = request.AccountNumber,
                    IsExistingCustomer = true,
                };

                var profileResponse = await _customerHelper.CreateCustomer(customer);
                if (!profileResponse.Status)
                    throw new CustomException("Attempt to create customer profile failed");
                var deviceCreationResponse = await _customerHelper.CreateDevice(customer.Id, request.DeviceModel, request.CountryCode);
                if (!deviceCreationResponse)
                    throw new CustomException("Weird!. Device failed after profile has been successfully created");
                resp.Data.Stage = RegistrationStage.ProfileCreated.GetDescription();
            }
            return resp;
        }

    }
}
