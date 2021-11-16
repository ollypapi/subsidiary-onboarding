using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Extentions;
using Application.Helper;
using Domain.Enum;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
    public class UpdateCustomerStatusCommand : IRequest<ResponseModel>
    {
        public string AccountNumber { get; set; }
        public CustomerStatus Status { get; set; }
    
    }
    public class UpdateCustomerStatusCommandValidator : AbstractValidator<UpdateCustomerStatusCommand>
    {
        public UpdateCustomerStatusCommandValidator()
        {
            RuleFor(c => c.AccountNumber).NotEmpty();
           
        }
    }
    public class UpdateCustomerStatusCommandHandler : IRequestHandler<UpdateCustomerStatusCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        public UpdateCustomerStatusCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomerByAccountNumber(request.AccountNumber);
            if(customer == null)
                throw new CustomException("Customer not Found");
            customer.Status = request.Status.GetDescription();
            return await _customerHelper.UpdateCustomer(request.AccountNumber, customer);
        }
    }
   
}
