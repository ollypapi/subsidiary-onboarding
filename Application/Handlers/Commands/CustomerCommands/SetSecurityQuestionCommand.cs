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
   public class SetSecurityQuestionCommand : IRequest<ResponseModel>
    {
        [JsonIgnore]
        public long CustomerId { get; set; }
        public List<QuestionModel> SecurityQuestions { get; set; }

    }
    public class SetSecurityQuestionCommandValidator : AbstractValidator<SetSecurityQuestionCommand>
    {
        public SetSecurityQuestionCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.SecurityQuestions.Count).GreaterThan(0);
        }   
    }
    public class SetSecurityQuestionCommandHandler : IRequestHandler<SetSecurityQuestionCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly SecurityQuestionHelper _securityQuestionHelper;
        public SetSecurityQuestionCommandHandler(CustomerHelper customerHelper, SecurityQuestionHelper securityQuestionHelper)
        {
            _customerHelper = customerHelper;
            _securityQuestionHelper = securityQuestionHelper;
        }
        public async Task<ResponseModel> Handle(SetSecurityQuestionCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomer(request.CustomerId);
            if (customer == null)
                throw new CustomException("Profile not found");
         
            return await _securityQuestionHelper.CreateSecurityQuestion(customer, request.SecurityQuestions);
        }
    }
}
