using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
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
    public class ValidateSecurityQuestionAnswerCommand: IRequest<ResponseModel<SecurityQuestionAnswerValidationResponse>>
    {
        public long QuestionId { get; set; }
        public string Answer { get; set; }
    }

    public class ValidateSecurityQuestionAnswerCommandValidator : AbstractValidator<ValidateSecurityQuestionAnswerCommand>
    {
        public ValidateSecurityQuestionAnswerCommandValidator()
        {
            RuleFor(c => c.Answer).NotEmpty();
            RuleFor(c => c.QuestionId).NotEmpty();
        }
    }

    public class ValidateSecurityQuestionAnswerCommandHandler : IRequestHandler<ValidateSecurityQuestionAnswerCommand, ResponseModel<SecurityQuestionAnswerValidationResponse>>
    {
        private readonly SecurityQuestionHelper _securityQuestionHelper;
        public ValidateSecurityQuestionAnswerCommandHandler(SecurityQuestionHelper securityQuestionHelper)
        {
            _securityQuestionHelper = securityQuestionHelper;
        }
        public async Task<ResponseModel<SecurityQuestionAnswerValidationResponse>> Handle(ValidateSecurityQuestionAnswerCommand request, CancellationToken cancellationToken)
        {
            var question = await _securityQuestionHelper.VerifyQuestionAnswer(request.QuestionId, request.Answer);
            return ResponseModel<SecurityQuestionAnswerValidationResponse>.Success(new SecurityQuestionAnswerValidationResponse { CustomerId = question.CustomerId, PhoneNumber = question.Customer.PhoneNumber });
        }
    }
}
