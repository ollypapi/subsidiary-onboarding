using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Helper;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
   public class ChangePasswordCommand : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.CurrentPassword).NotEmpty();
            RuleFor(c => c.NewPassword).NotEmpty();
        }   
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly DocumentHelper _documentHelper;
        public ChangePasswordCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomer(request.CustomerId);
            if (customer == null)
                throw new CustomException("User not found");
            if (!AppUtility.VerifyInformation(request.CurrentPassword, customer.PasswordSalt, customer.PasswordHash))
                throw new CustomException("Your current password could not be validated");
            return await _customerHelper.SetPassword(customer, request.NewPassword);
        }
    }
}
