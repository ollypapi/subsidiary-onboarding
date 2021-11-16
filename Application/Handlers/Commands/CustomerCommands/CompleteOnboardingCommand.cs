using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Helper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Handlers.Commands.CustomerCommands
{
   public class CompleteOnboardingCommand : IRequest<ResponseModel>
    {
        public string OTPTrackingId { get; set; }
        public string  TransactionPin { get; set; }
        public string Password { get; set; }
        public IdentificationModel MeansOfIdentification { get; set; }
        public List<QuestionModel> SecurityQuestion { get; set; }

    }
    public class CompleteOnboardingCommandHandler : IRequestHandler<CompleteOnboardingCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly DocumentHelper _documentHelper;
        public CompleteOnboardingCommandHandler(CustomerHelper customerHelper)
        {
            _customerHelper = customerHelper;
        }
        public async Task<ResponseModel> Handle(CompleteOnboardingCommand request, CancellationToken cancellationToken)
        {
            return ResponseModel.Success("Successful");
        }
    }
}
