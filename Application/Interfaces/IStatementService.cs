using Application.Common.Models.Response;
using Application.Common.Models.StatementServive;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStatementService
    {
        Task<NTransactionResponse> GetNTransactions(GetNTransactionRequest request);
    }
}
