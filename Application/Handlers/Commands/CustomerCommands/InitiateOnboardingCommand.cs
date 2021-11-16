using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Extentions;
using Application.Helper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
   public class InitiateOnboardingCommand:IRequest<ResponseModel>
    {
        public ProfileModel CustomerProfile { get; set; }
        public DeviceModel DeviceModel { get; set; } 

    }
    public class InitiateOnboardingValidator : AbstractValidator<InitiateOnboardingCommand>
    {
        public InitiateOnboardingValidator()
        {
            RuleFor(c => c.CustomerProfile).NotNull();
            RuleFor(c => c.DeviceModel).NotNull();
        }

    }
    public class CreateCustomerProfileCommandHandler : IRequestHandler<InitiateOnboardingCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly DocumentHelper _documentHelper;
        public CreateCustomerProfileCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(InitiateOnboardingCommand request, CancellationToken cancellationToken)
        {
            var isDeviceExisting = await _customerHelper.isDeviceExisting(request.DeviceModel.DeviceId);
            if (isDeviceExisting)
                throw new CustomException("Device mismatch");
            if (request.CustomerProfile.ReferralCode != string.Empty)
            {
                var referralCodeIsValid = await _customerHelper.ReferralCodeExist(request.CustomerProfile.ReferralCode);
                if (!referralCodeIsValid)
                    throw new CustomException("Please enter a valid referral code..");
            }
            var customer = new Customer
            {
                Branch = request.CustomerProfile.Branch,
                DateCreated = DateTime.Now,
                DOB = request.CustomerProfile.DOB,
                Email = request.CustomerProfile.Email,
                DeviceId=request.DeviceModel.DeviceId,
                FirstName = request.CustomerProfile.FirstName,
                LastName = request.CustomerProfile.LastName,
                Gender = request.CustomerProfile.Gender,
                PhoneNumber = request.CustomerProfile.PhoneNumber,
                ReferredBy=request.CustomerProfile.ReferralCode,
                MiddleName = request.CustomerProfile.MiddleName,
                Stage = Domain.Enum.RegistrationStage.ProfileCreated,
                Address = request.CustomerProfile.Address,
                State = request.CustomerProfile.Region,
                Title = request.CustomerProfile.Title,
                Nationality = request.CustomerProfile.Nationality,
                MaritalStatus = request.CustomerProfile.MaritalStatus,
                Occupation = request.CustomerProfile.Occupation,
                Region = request.CustomerProfile.Region,
                Town = request.CustomerProfile.Town,
                IdType = request.CustomerProfile.IdType,
                CountryId = request.CustomerProfile.CountryId,
                Status = Domain.Enum.CustomerStatus.ACTIVATED.GetDescription(),
                SubsidiaryId = request.CustomerProfile.CountryId
            };

            var profileResponse = await _customerHelper.CreateCustomer(customer);
            if (!profileResponse.Status)
                throw new CustomException("Attempt to create customer profile failed");
            var deviceCreationResponse = await _customerHelper.CreateDevice(customer.Id, request.DeviceModel, request.CustomerProfile.CountryId);
            if (!deviceCreationResponse)
                throw new CustomException("Weird!. Device failed after profile has been successfully created");
            return ResponseModel.Success("Profile Created Successful");
        }
    }
}
