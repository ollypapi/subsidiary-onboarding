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
   public class ChangeTransactionPinCommand : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public string CurrentTransactionPin { get; set; }
        public string NewTransactionPin { get; set; }
    }
    public class ChangeTransactionPinCommandValidator : AbstractValidator<ChangeTransactionPinCommand>
    {
        public ChangeTransactionPinCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.CurrentTransactionPin).NotEmpty();
            RuleFor(c => c.NewTransactionPin).NotEmpty();
        }   
    }
    public class ChangeTransactionPinCommandHandler : IRequestHandler<ChangeTransactionPinCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly DocumentHelper _documentHelper;
        public ChangeTransactionPinCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(ChangeTransactionPinCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomer(request.CustomerId);
            if (customer == null)
                throw new CustomException("User not found");

            if (!AppUtility.VerifyInformation(request.CurrentTransactionPin, customer.TransactionPinSalt, customer.TransactionPin))
                throw new CustomException("Your current transaction pin could not be validated");
            return await _customerHelper.SetTransactionPin(customer, request.NewTransactionPin);
        }
    }
}
