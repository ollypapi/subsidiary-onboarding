using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Helper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{

    public class FinalizeOnboardingCommand : IRequest<ResponseModel<AccountCreationResponse>>
    {
        public string PhoneNumber { get; set; }
        public string CountryId { get; set; }
    }
    public class FinalizeOnboardingCommandValidator : AbstractValidator<FinalizeOnboardingCommand>
    {
        public FinalizeOnboardingCommandValidator()
        {
            RuleFor(c => c.PhoneNumber).NotEmpty();
            RuleFor(c => c.CountryId).NotEmpty();
        }
    }
    public class FinalizeOnboardingCommandHandler : IRequestHandler<FinalizeOnboardingCommand, ResponseModel<AccountCreationResponse>>
    {
        private readonly AccountHelper _accountHelper;
        public FinalizeOnboardingCommandHandler(AccountHelper accountHelper)
        {
            _accountHelper = accountHelper;
        }
        public async Task<ResponseModel<AccountCreationResponse>> Handle(FinalizeOnboardingCommand request, CancellationToken cancellationToken)
        {
            return await _accountHelper.CreateCustomerAccount(request.PhoneNumber, request.CountryId);
        }
    }
}
