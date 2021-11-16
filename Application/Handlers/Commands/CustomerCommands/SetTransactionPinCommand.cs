using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Helper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
   public class SetTransactionPinCommand : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public string TransactionPin { get; set; }
    }
    public class SetTransactionPinCommandValidator : AbstractValidator<SetTransactionPinCommand>
    {
        public SetTransactionPinCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.TransactionPin).NotEmpty();
        }   
    }
    public class SetTransactionPinCommandHandler : IRequestHandler<SetTransactionPinCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        public SetTransactionPinCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(SetTransactionPinCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomer(request.CustomerId);
            if (customer == null)
                throw new CustomException("User not found");
            if (customer.Stage == Domain.Enum.RegistrationStage.PinSet)
                throw new CustomException("Pin already set. Please do password reset in case you have forgotten your password");
            return await _customerHelper.SetTransactionPin(customer, request.TransactionPin);
        }
    }
}
