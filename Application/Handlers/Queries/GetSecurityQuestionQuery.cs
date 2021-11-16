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

namespace Application.Handlers.Queries
{
    public class GetSecurityQuestionQuery : IRequest<ResponseModel<SecurityQuestionResponse>>
    {
        public string AccountNumber { get; set; }
        public string CountryCode { get; set; }

    }

    public class GetSecurityQuestionQueryValidator : AbstractValidator<GetSecurityQuestionQuery>
    {
        public GetSecurityQuestionQueryValidator()
        {
            RuleFor(c => c.AccountNumber).NotEmpty();
            RuleFor(c => c.CountryCode).NotEmpty();
        }
    }
    public class GetSecurityQuestionQueryHandler : IRequestHandler<GetSecurityQuestionQuery, ResponseModel<SecurityQuestionResponse>>
    {
        private readonly SecurityQuestionHelper _securityQuestionHelper;

        public GetSecurityQuestionQueryHandler(SecurityQuestionHelper securityQuestionHelper)
        {
            _securityQuestionHelper = securityQuestionHelper;
        }
        public async Task<ResponseModel<SecurityQuestionResponse>> Handle(GetSecurityQuestionQuery request, CancellationToken cancellationToken)
        {
            var securityQuestions = await _securityQuestionHelper.GetSecurityQuestionByAccountNumber(request.AccountNumber, request.CountryCode);
            if (securityQuestions.Count == 0)
                throw new CustomException("Customer does not have Security Question");
            var random = new Random();
            int index = random.Next(securityQuestions.Count);
            var question = securityQuestions[index];
            return ResponseModel<SecurityQuestionResponse>.Success(new SecurityQuestionResponse { Id = question.Id,Question= question.Question });
        }


    }
}
