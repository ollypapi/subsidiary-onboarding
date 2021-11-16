using Application.Common.Enums;
using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Extensions
{
    public static class SendMailExtention
    {
        public static void AddMessage(this SendMailModel sendMailModel, NotificationTypeEnum notificationType, string AccountNumber = null, string Token=null, string Device=null)
        {
            string message = notificationType switch
                {
                    NotificationTypeEnum.AccountCreated => $" A new Account {AccountNumber} has been created, Kindly Check the KYC document ",
                    NotificationTypeEnum.AccountActivated => $"Congratulation Your Account {AccountNumber}, has been Activated",
                    NotificationTypeEnum.AccountRejected => $"Sorry, Your Account has been restricted, Kindly Consult your account Officer or Visit one of our branch around you",
                    NotificationTypeEnum.DeviceDeactivated => $"Sorry, Your Device {Device} has been Deactivated",
                    NotificationTypeEnum.DeviceActivated => $"Great, Your Device {Device} has been Activated",
                    NotificationTypeEnum.DeviceRelease => $"Sorry, Your Device {Device} has been totally Released",
                    NotificationTypeEnum.DocumentActivated => $"Congratulations, One of your KYC document has been Accepted ",
                    NotificationTypeEnum.DocumentRejected => $"Sorry, Your KYC document was rejected, Kindly login to your Mobile banking app to Update your KYC document ",
                    NotificationTypeEnum.Token => $"Do not share this code with anyone, including us. Your verification code is {Token}.",
                    _ => string.Empty,
                };

            sendMailModel.Message = message;
           
        }

        public static void AddSubject(this SendMailModel sendMailModel, NotificationTypeEnum notificationType)
        {
            string Subject = notificationType switch
            {
                NotificationTypeEnum.AccountCreated => $"Account Created",
                NotificationTypeEnum.AccountActivated => $"Account Activated",
                NotificationTypeEnum.AccountRejected => $"Account Rejected",
                NotificationTypeEnum.DeviceDeactivated => $"Device Deactivated",
                NotificationTypeEnum.DeviceActivated => $"Device Activated",
                NotificationTypeEnum.DeviceRelease => $"Device Released",
                NotificationTypeEnum.DocumentActivated => $"Document Accepted ",
                NotificationTypeEnum.DocumentRejected => $"Document Rejected",
                NotificationTypeEnum.Token => $"Authentication Token",
                _ => string.Empty,
            };

            sendMailModel.Subject = Subject;
        }
    }
}
