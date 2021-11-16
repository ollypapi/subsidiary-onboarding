using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Helper;
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
    public class UpdateSecurityQuestionCommand: IRequest<ResponseModel>
    {
        public string CustomerId { get; set; }
        public string TrackingId { get; set; }
        public string OTP { get; set; }
        public List<QuestionModel> SecurityQuestions { get; set; }
    }

    public class UpdateSecurityQuestionCommandValidator : AbstractValidator<UpdateSecurityQuestionCommand>
    {
        public UpdateSecurityQuestionCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
            RuleFor(c => c.TrackingId).NotEmpty();
            RuleFor(c => c.SecurityQuestions.Count).GreaterThan(0);
        }
    }

    public class UpdateSecurityQuestionCommandHandler : IRequestHandler<UpdateSecurityQuestionCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly SecurityQuestionHelper _securityQuestionHelper;
        private readonly VerificationHelper _verificationHelper;
        public UpdateSecurityQuestionCommandHandler(CustomerHelper customerHelper, SecurityQuestionHelper securityQuestionHelper, VerificationHelper verificationHelper)
        {
            _customerHelper = customerHelper;
            _securityQuestionHelper = securityQuestionHelper;
            _verificationHelper = verificationHelper;
        }
        public async Task<ResponseModel> Handle(UpdateSecurityQuestionCommand request, CancellationToken cancellationToken)
        {

            var customer = await _customerHelper.GetCustomerByFICustomerId(request.CustomerId);
            if (customer == null)
                throw new CustomException("Profile not found");
            await _verificationHelper.VerifyGeneralOtp(request.TrackingId, request.OTP, Domain.Enum.OtpPurpose.SecurityQuestion);
            var questions = await _securityQuestionHelper.GetSecurityQuestionByAccountNumber(customer.AccountNumber);
             await _securityQuestionHelper.DeleteSecurityQuestions(questions);
            return await _securityQuestionHelper.CreateSecurityQuestion(customer.Id, request.SecurityQuestions);
        }
    }
}
