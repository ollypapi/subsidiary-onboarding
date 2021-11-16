using Application.Common.Models;
using Application.Extentions;
using Application.Helper;
using Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Queries
{
    public class GetCustomerDocumentsQuery : IRequest<ResponseModel<List<CustomerDocumentModel>>>
    {
        public string AccountNumber { get; set; }
    }
    public class GetCustomerDocumentsQueryValidator : AbstractValidator<GetCustomerDocumentsQuery>
    {
        public GetCustomerDocumentsQueryValidator()
        {
            RuleFor(c => c.AccountNumber).NotEmpty();
        }
    }
    public class GetCustomerDocumentsQueryHanndler : IRequestHandler<GetCustomerDocumentsQuery, ResponseModel<List<CustomerDocumentModel>>>
    {
        private readonly DocumentHelper _documentHelper;
        private readonly IConfiguration _configuration;
        public GetCustomerDocumentsQueryHanndler(DocumentHelper documentHelper, IConfiguration configuration)
        {
            _documentHelper = documentHelper;
            _configuration = configuration;
        }
       

        public async Task<ResponseModel<List<CustomerDocumentModel>>> Handle(GetCustomerDocumentsQuery request, CancellationToken cancellationToken)
        {
            var data = await  _documentHelper.GetCustomerDocuments(request.AccountNumber);
            string baseUrl = $"{_configuration.GetValue<string>("Util:FileManagerBase")}";
            List<CustomerDocumentModel> Datamodel = new List<CustomerDocumentModel>();
            data.ForEach(d => {
                Datamodel.Add(new CustomerDocumentModel { Id = d.Id, CustomerId = d.CustomerId, DocumentUrl = $"{baseUrl}/files/{d.DocumentUrl}", DocumentName = d.DocumentName, DocumentType = d.DocumentType != null ? d.DocumentType.GetDescription() : DocumentType.Identity.GetDescription(), Identification = d.Identification, IdentificationId = d.IdentificationId,IdentificationNumber = d.IdentificationNumber, LastUpdatedDate = d.LastUpdatedDate,Status= d.Status == DocumentStatus.Approved || d.Status == DocumentStatus.Pending? d.Status.GetDescription(): DocumentStatus.Rejected.GetDescription() , DateCreated = d.DateCreated });
            });
            return ResponseModel<List<CustomerDocumentModel>>.Success(Datamodel);
        }
    }

   
}
