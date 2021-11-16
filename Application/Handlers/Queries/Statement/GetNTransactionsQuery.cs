using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Common.Models.StatementServive;
using Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries.Statement
{
    public class GetNTransactionsQuery : GetNTransactionRequest, IRequest<ResponseModel<NTransactionResponse>>
    {

    }

    public class GetNTransactionsQueryHandler : IRequestHandler<GetNTransactionsQuery, ResponseModel<NTransactionResponse>>
    {
        private readonly IStatementService _statementService;
        public GetNTransactionsQueryHandler(IStatementService statementService)
        {
            _statementService = statementService;
        }
        public async Task<ResponseModel<NTransactionResponse>> Handle(GetNTransactionsQuery request, CancellationToken cancellationToken)
        {
            var data = await _statementService.GetNTransactions(request);
            return ResponseModel<NTransactionResponse>.Success(data);
        }
    }


}
