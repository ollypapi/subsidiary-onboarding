using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
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
   public class ResumeOnboardingCommand : IRequest<ResponseModel<OnboardingPriorityResponse>>
    {
  
        public string PhoneNumber { get; set; }
    }
    public class ResumeOnboardingCommandValidator : AbstractValidator<ResumeOnboardingCommand>
    {
        public ResumeOnboardingCommandValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
        }   
    }
    public class ResumeOnboardingCommandHandler : IRequestHandler<ResumeOnboardingCommand, ResponseModel<OnboardingPriorityResponse>>
    {
        private readonly CustomerHelper _customerHelper;
        public ResumeOnboardingCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel<OnboardingPriorityResponse>> Handle(ResumeOnboardingCommand request, CancellationToken cancellationToken)
        {

            var customer = await _customerHelper.GetCustomerFromPhone(request.PhoneNumber);
            if (customer == null)
                throw new CustomException("Profile not found");

            if (customer.Stage == Domain.Enum.RegistrationStage.OnboardingCompleted)
                throw new CustomException("Onboarding stage has been completed. Please proceed to login");

            var token =  _customerHelper.GenerateToken(customer, JwtUserType.AnonynmousUser);
            var stage =customer.Stage.GetDescription();
            var onboardingPriority = new OnboardingPriorityResponse { OnboardingPriority = stage, Token=token.Data };
            return ResponseModel<OnboardingPriorityResponse>.Success(onboardingPriority);
        }
    }
}
