using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Extensions;
using Application.Helper;
using Application.Interfaces;
using Domain.Enum;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
    public class UpdateDocumentStatusCommand : IRequest<ResponseModel>
    {

        public long DocumentId { get; set; }
        public DocumentStatus Status { get; set; }
    }
    public class UpdateDocumentStatusCommandValidator : AbstractValidator<UpdateDocumentStatusCommand>
    {
        public UpdateDocumentStatusCommandValidator()
        {
            RuleFor(c => c.DocumentId).NotEmpty();
            RuleFor(c => c.Status).NotNull().NotEmpty();
        }
    }

    public class UpdateDocumentStatusCommandHandler : IRequestHandler<UpdateDocumentStatusCommand, ResponseModel>
    {
        private readonly DocumentHelper _documentHelper;
        private readonly IAccountService _accountService;
        private readonly CustomerHelper _customerHelper;
        private readonly INotificationService _notificationService;

        public UpdateDocumentStatusCommandHandler(DocumentHelper documentHelper, IAccountService accountService, CustomerHelper customerHelper, INotificationService notificationService)
        {
            _documentHelper = documentHelper;
            _accountService = accountService;
            _customerHelper = customerHelper;
            _notificationService = notificationService;
        }
        public async Task<ResponseModel> Handle(UpdateDocumentStatusCommand request, CancellationToken cancellationToken)
        {
            var document = await _documentHelper.GetCustomerDocument(request.DocumentId);
            if (document == null)
                throw new CustomException("Document not Found");
            document.Status = request.Status;
            await _documentHelper.UpdateCustomerDocument(document);

            var allDocuments = await _documentHelper.GetCustomerDocuments(document.CustomerId);
            var customer = await _customerHelper.GetCustomer(document.CustomerId);
            // check if all documents are approved, if true, remove PND on Account
            if (allDocuments.TrueForAll(doc=>doc.Status == DocumentStatus.Approved))
            {
               
                await _accountService.RemovePNDOnAccount(new PNDRequest { AccountNumber = customer.AccountNumber, FreezeReason = "Remove PND", CountryId = customer.CountryId });
            }

            var subject = request.Status == DocumentStatus.Approved ? NotificationTypeEnum.DocumentActivated : NotificationTypeEnum.DocumentRejected;
            SendMailModel sendMailModel = new SendMailModel { Recipient = customer.Email };
            sendMailModel.AddSubject(subject);
            sendMailModel.AddMessage(subject, null, null,null);
            sendMailModel.Recipient = customer.Email;
            sendMailModel.Template = new Dictionary<string, string>
                    {
                        { "customerName", $"{customer.FirstName} {customer.LastName}" },
                        { "messageContent", sendMailModel.Message },
                        { "senderName", sendMailModel.Subject }
                    };

            await _notificationService.SendMail(sendMailModel);

            return ResponseModel.Success();
        }
    }
}
