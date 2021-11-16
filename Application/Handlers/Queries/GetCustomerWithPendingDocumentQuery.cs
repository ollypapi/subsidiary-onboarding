﻿using Application.Common.Models;
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
    public class GetCustomerWithPendingDocumentQuery : IRequest<ResponseModel<List<CustomerModel>>>
    {
        [JsonIgnore]
        public string CountryCode { get; set; }
    }

    public class GetCustomerWithPendingDocumentQueryHandler : IRequestHandler<GetCustomerWithPendingDocumentQuery, ResponseModel<List<CustomerModel>>>
    {
        private readonly CustomerHelper _customerHelper;
        public GetCustomerWithPendingDocumentQueryHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper; ;
        }
        public async Task<ResponseModel<List<CustomerModel>>> Handle(GetCustomerWithPendingDocumentQuery request, CancellationToken cancellationToken)
        {
            var data = await _customerHelper.GetCustomerWithPendingDocuments(request.CountryCode);
            var customers = new List<CustomerModel>();
            data.ForEach(c =>
            {
                customers.Add(new CustomerModel
                {
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
                    DateCreated = c.DateCreated
                });
            });
            return ResponseModel<List<CustomerModel>>.Success(customers);
        }
    }
    
}
