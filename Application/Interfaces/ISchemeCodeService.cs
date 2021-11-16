using Application.Common.Models;
using Application.Common.Models.AccountService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISchemeCodeService
    {
        Task<ResponseModel> AddSchemeCode(SchemeCode request);
        Task<ResponseModel> UpdateSchemeCode(SchemeCode request);
        Task<ResponseModel<List<SchemeCode>>> GetCountrySchemeCodes(string CountryCode);
    }
}
