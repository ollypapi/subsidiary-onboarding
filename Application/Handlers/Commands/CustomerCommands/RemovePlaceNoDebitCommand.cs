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
    public class RemovePlaceNoDebitCommand: PNDRequest,  IRequest<ResponseModel>
    {
       
    }

    public class RemovePlaceNoDebitCommandValidator : AbstractValidator<RemovePlaceNoDebitCommand>
    {
        public RemovePlaceNoDebitCommandValidator()
        {
            RuleFor(c => c.AccountNumber).NotEmpty();
            RuleFor(c => c.FreezeReason).NotEmpty();
        }
    }

    public class RemovePlaceNoDebitCommandHandler : IRequestHandler<RemovePlaceNoDebitCommand, ResponseModel>
    {
        private readonly IAccountService _accountService;
        
        public RemovePlaceNoDebitCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<ResponseModel> Handle(RemovePlaceNoDebitCommand request, CancellationToken cancellationToken)
        {
            var resp = await _accountService.RemovePNDOnAccount(request);
            if (resp.ResponseCode != "00")
                throw new CustomException(resp.ResponseMessage);
            return ResponseModel.Success(resp.ResponseMessage);
        }
    }

}
