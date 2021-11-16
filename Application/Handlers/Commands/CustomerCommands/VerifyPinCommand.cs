using Application.Common.Models;
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
    public class VerifyPinCommand: IRequest<ResponseModel>
    {
        public string CustomerId { get; set; } 
        public string TransactionPin { get; set; }
    }

    public class VerifyPinCommandValidator : AbstractValidator<VerifyPinCommand>
    {
        public VerifyPinCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.TransactionPin).NotEmpty();
        }
    }

    public class VerifyPinCommandHandler : IRequestHandler<VerifyPinCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;

        public VerifyPinCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }

        public async Task<ResponseModel> Handle(VerifyPinCommand request, CancellationToken cancellationToken)
        {
           return await  _customerHelper.VerifyPin(request.CustomerId, request.TransactionPin);

        }
    }

}
