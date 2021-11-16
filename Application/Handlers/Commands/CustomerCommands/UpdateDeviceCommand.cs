using Application.Common.Enums;
using Application.Common.Models;
using Application.Extensions;
using Application.Helper;
using Application.Interfaces;
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
    public class UpdateDeviceCommand : IRequest<ResponseModel>
    {
        public string DeviceId { get; set; }
        public string Status { get; set; }
        public string CustomerId { get; set; }
    }

    public class UpdateDeviceCommandValidator : AbstractValidator<UpdateDeviceCommand>
    {
        public UpdateDeviceCommandValidator()
        {
            RuleFor(c => c.DeviceId).NotEmpty();
            RuleFor(c => c.Status).NotEmpty();
            RuleFor(c => c.CustomerId).NotEmpty();
        }
    }
    public class UpdateDeviceCommandValidatorHandler : IRequestHandler<UpdateDeviceCommand, ResponseModel>
    {
        private readonly CustomerHelper _customerHelper;
        private readonly INotificationService _notificationService;
        public UpdateDeviceCommandValidatorHandler(CustomerHelper customerHelper, INotificationService notification)
        {
            _customerHelper = customerHelper;
            _notificationService = notification;

        }
        public async Task<ResponseModel> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerHelper.GetCustomerByFICustomerId(request.CustomerId);
            var device = await _customerHelper.GetCustomerDevice(request.DeviceId, customer.Id);
           
            device.Status = request.Status;

            _customerHelper.UpdateCustomerDevice(device);
            var subject = request.Status.ToLowerInvariant().Equals("activated") ? NotificationTypeEnum.DeviceActivated : request.Status.ToLowerInvariant().Equals("released")?NotificationTypeEnum.DeviceRelease:NotificationTypeEnum.DeviceDeactivated;
            SendMailModel sendMailModel = new SendMailModel { Recipient = customer.Email };
            sendMailModel.AddSubject(subject);
            sendMailModel.AddMessage(subject, null, null,device.DeviceName);
            sendMailModel.Recipient = customer.Email;
            sendMailModel.Template = new Dictionary<string, string>
                    {
                        { "customerName", $"{customer.FirstName} {customer.LastName}" },
                        { "messageContent", sendMailModel.Message },
                        { "senderName", sendMailModel.Subject }
                    };

            if(customer.Email != null)
            await _notificationService.SendMail(sendMailModel);

            return ResponseModel.Success();
        }
    }
}
