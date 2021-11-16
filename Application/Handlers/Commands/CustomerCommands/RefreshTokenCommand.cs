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
    public class RefreshTokenCommand: IRequest<ResponseModel<LoginResponse>>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(c => c.RefreshToken).NotEmpty();
        }
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseModel<LoginResponse>>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly ActivityLogHelper _activityLog;
        public RefreshTokenCommandHandler(CustomerHelper customerHelper, ActivityLogHelper activityLog)
        {
            _customerHelper = customerHelper;
            _activityLog = activityLog;
        }
        public async Task<ResponseModel<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _customerHelper.RefreshAuthToken(request.RefreshToken);

            if (result.Status.Equals(true))
                _activityLog.ActivityResult = result.Message;

            _activityLog.ResultDescription = "Request call to Referesh token";
            _activityLog.LogCurrentActivity("Refresh token",
                    new UserActivityRequestModel
                    {
                        AccountNumber = request.RefreshToken,
                    });

            return result;

        }
    }
}
