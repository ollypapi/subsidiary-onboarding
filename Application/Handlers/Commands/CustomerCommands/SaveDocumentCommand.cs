using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Helper;
using Domain.Entities;
using Domain.Enum;
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
    public class SaveDocumentCommand : IRequest<ResponseModel>
    {
        public string PhoneNumber { get; set; }
        
        public IdentificationModel MeansOfIdentification { get; set; }
        public List<DocumentModel> Documents { get; set; }

    }
    public class SaveDocumentCommandValidator : AbstractValidator<SaveDocumentCommand>
    {
        public SaveDocumentCommandValidator()
        {
            RuleFor(c => c.Documents).NotNull();
            RuleFor(c => c.MeansOfIdentification).NotNull();
        }

    }
    public class SaveDocumentCommandHandler : IRequestHandler<SaveDocumentCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly DocumentHelper _documentHelper;
        public SaveDocumentCommandHandler(CustomerHelper customerHelper, DocumentHelper documentHelper)
        {
            _customerHelper = customerHelper;
            _documentHelper = documentHelper;
        }
        public async Task<ResponseModel> Handle(SaveDocumentCommand request, CancellationToken cancellationToken)
        {

            var customer = await _customerHelper.GetCustomerFromPhone(request.PhoneNumber);
            if (customer == null)
                throw new CustomException("Customer details not found");
           
            if(customer.Stage!=Domain.Enum.RegistrationStage.ProfileCreated)
                throw new CustomException("Kindly review your current stage");

            //var profileResponse = await _customerHelper.CreateCustomer(customer);

            //if (!profileResponse.Status)
            //    throw new CustomException("Attempt to create customer profile failed");

            var identityResponse = await _documentHelper.SaveIdentityDocument(customer.Id, request.MeansOfIdentification);
            var documentStatus = await _documentHelper.SaveDocument(customer.Id, request.Documents);

            if (identityResponse.Status != true || documentStatus.Status != true)
                throw new CustomException("Profile Registration was created with incomplete status");
            customer.Stage = RegistrationStage.DocumentUploadCompleted;
            await _customerHelper.UpdateCustomerStage(customer.Id, RegistrationStage.DocumentUploadCompleted);
            return ResponseModel.Success("Document(s) uploaded Successfully");
        }
    }
}
