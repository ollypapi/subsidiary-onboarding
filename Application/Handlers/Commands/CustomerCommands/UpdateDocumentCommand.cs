using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Extentions;
using Application.Helper;
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
    public class UpdateDocumentCommand : IRequest<ResponseModel>
    {
        public long DocumentId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentUrl { get; set; }
        public string DocumentFolder { get; set; }
        public string Identification { get; set; }
        public string IdentificationId { get; set; }
        public string IdentificationNumber { get; set; }
    }
    public class UpdateDocumentCommandValidator : AbstractValidator<UpdateDocumentCommand>
    {
        public UpdateDocumentCommandValidator()
        {
            RuleFor(c => c.DocumentId).NotEmpty();
            RuleFor(c => c.DocumentUrl).NotNull().NotEmpty();
            RuleFor(c => c.DocumentName).NotNull().NotEmpty();
        }
    }
    public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, ResponseModel>
    {
        private readonly DocumentHelper _documentHelper;
        public UpdateDocumentCommandHandler(DocumentHelper documentHelper)
        {
            _documentHelper = documentHelper;
        }
        public async Task<ResponseModel> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = await _documentHelper.GetCustomerDocument(request.DocumentId);
            if (document == null)
                throw new CustomException("Document not Found");
            document.Status = DocumentStatus.Pending;
            document.DocumentName = request.DocumentName != null ? request.DocumentName : document.DocumentName;
            document.DocumentUrl = $"{document.DocumentFolder}/{request.DocumentName}";
            await _documentHelper.UpdateCustomerDocument(document);
            return ResponseModel.Success();
        }
    }
}
