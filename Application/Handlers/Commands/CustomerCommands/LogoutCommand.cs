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

    public class LogoutCommand : IRequest<ResponseModel>
    {
        public long CustomerId { get; set; }
    }

    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(c => c.CustomerId).NotEmpty();
        }
    }

    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly ActivityLogHelper _activityLog;
        public LogoutCommandHandler(CustomerHelper customerHelper, ActivityLogHelper activityLog)
        {
            _customerHelper = customerHelper;
            _activityLog = activityLog;
        }
        public async Task<ResponseModel> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var result = await _customerHelper.Logout(request.CustomerId);
            return result;
        }
    }

}
