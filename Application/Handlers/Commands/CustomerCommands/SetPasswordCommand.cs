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
   public class SetPasswordCommand : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public string Password { get; set; }
    }
    public class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand>
    {
        public SetPasswordCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.Password).NotEmpty();
        }   
    }
    public class SetPasswordCommandHandler : IRequestHandler<SetPasswordCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly DocumentHelper _documentHelper;
        public SetPasswordCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomer(request.CustomerId);
            if (customer == null)
                throw new CustomException("User not found");
            if (customer.Stage == Domain.Enum.RegistrationStage.PasswordSet)
                throw new CustomException("Password already set. Please do password reset in case you have forgotten your password");
            return await _customerHelper.SetPassword(customer, request.Password);
        }
    }
}
