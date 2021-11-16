using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{

    public class GetCustomerByAccountNumberQuery : IRequest<ResponseModel<CustomerModel>>
    {
        [JsonIgnore]
        public string AccountNumber { get; set; }
        [JsonIgnore]
        public string Countrycode { get; set; }
    }

    public class GetCustomerByAccountNumberQueryHandler : IRequestHandler<GetCustomerByAccountNumberQuery, ResponseModel<CustomerModel>>
    {
        private readonly CustomerHelper _customerHelper;
        public GetCustomerByAccountNumberQueryHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper; ;
        }
        public async Task<ResponseModel<CustomerModel>> Handle(GetCustomerByAccountNumberQuery request, CancellationToken cancellationToken)
        {
            var c = await _customerHelper.GetCustomerByAccountNumberAndCountryCode(request.AccountNumber, request.Countrycode);
            var customer = new CustomerModel {
                Id = c.Id,
                AccountNumber = c.AccountNumber,
                Address = c.Address,
                Branch = c.Branch,
                CifId = c.CifId,
                CustomerId = c.CustomerId,
                DeviceId = c.DeviceId,
                DOB = c.DOB,
                Email = c.Email,
                FirstName = c.FirstName,
                Gender = c.Gender,
                IdType = c.IdType,
                LastName = c.LastName,
                MaritalStatus = c.MaritalStatus,
                MiddleName = c.MiddleName,
                Nationality = c.Nationality,
                Occupation = c.Occupation,
                PhoneNumber = c.PhoneNumber,
                ReferralCode = c.ReferralCode,
                ReferredBy = c.ReferredBy,
                Region = c.Region,
                Stage = c.Stage,
                State = c.State,
                SubsidiaryId = c.SubsidiaryId,
                Title = c.Title,
                Town = c.Town,
                Status = c.Status,
                DateCreated = c.DateCreated
            };
          
            return ResponseModel<CustomerModel>.Success(customer);
        }
    }

   
}
