using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
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
   public class LoginCommand : IRequest<ResponseModel<LoginResponse>>
    {
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string DeviceId { get; set; }
        public string CountryId { get; set; }
    }
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(c => c.Password).NotEmpty();
            RuleFor(c => c.DeviceId).NotEmpty();
            RuleFor(c => c.CountryId).NotEmpty();
        }   
    }
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ResponseModel<LoginResponse>>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly ActivityLogHelper _activityLog;
        public LoginCommandHandler(CustomerHelper customerHelper, ActivityLogHelper activityLog)
        {
            _customerHelper = customerHelper;
            _activityLog = activityLog;
        }
        public async Task<ResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            string username = request.Username != null ? request.Username : request.PhoneNumber;
            var result = await _customerHelper.Login(username,request.Password,request.DeviceId, request.CountryId);
            if (result.Status.Equals(true))
                _activityLog.ActivityResult = result.Message;

            _activityLog.ResultDescription = "Request call was on Sign in";
            _activityLog.LogCurrentActivity("Sign in",
                    new UserActivityRequestModel
                    {
                        AccountNumber = request.Username,
                    });

            return result;
               
        }
    }
}
