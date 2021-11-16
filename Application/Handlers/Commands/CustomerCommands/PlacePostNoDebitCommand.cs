using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{

    public class PlacePostNoDebitCommand : PNDRequest, IRequest<ResponseModel>
    {

    }

    public class PlacePostNoDebitCommandValidator : AbstractValidator<PlacePostNoDebitCommand>
    {
        public PlacePostNoDebitCommandValidator()
        {
            RuleFor(c => c.AccountNumber).NotEmpty();
            RuleFor(c => c.FreezeReason).NotEmpty();
        }
    }

    public class PlacePostNoDebitCommandHandler : IRequestHandler<PlacePostNoDebitCommand, ResponseModel>
    {
        private readonly IAccountService _accountService;

        public PlacePostNoDebitCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<ResponseModel> Handle(PlacePostNoDebitCommand request, CancellationToken cancellationToken)
        {
            var resp = await _accountService.RemovePNDOnAccount(request);
            if (resp.ResponseCode != "00")
                throw new CustomException(resp.ResponseMessage);
            return ResponseModel.Success(resp.ResponseMessage);
        }
    }
}
