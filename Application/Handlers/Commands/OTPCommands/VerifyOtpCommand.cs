using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Helper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.OTPCommands
{

    public class VerifyOtpCommand : IRequest<ResponseModel<Token>>
    {
        public string  Otp { get; set; }
        public string TrackingId { get; set; }

    }
    public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, ResponseModel<Token>>
    {
        private readonly VerificationHelper _verificationHelper;
        public VerifyOtpCommandHandler(VerificationHelper verificationHelper)
        {
            _verificationHelper = verificationHelper;
        }
        public async Task<ResponseModel<Token>> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
        {
           return await _verificationHelper.VerifyOtp(request.TrackingId,request.Otp);
        }
    }
}
