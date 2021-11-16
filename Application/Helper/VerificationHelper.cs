using App.Commands;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extensions;
using Application.Extentions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class VerificationHelper
    {
        private readonly IOnboardingDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly CustomerHelper _customerHelper;
        private readonly INotificationService _notificationService;
        public VerificationHelper(IOnboardingDbContext context, IConfiguration configuration,
            IMediator mediator, CustomerHelper customerHelper, INotificationService notificationService)
        {
            _customerHelper = customerHelper;
            _mediator = mediator;
            _context = context;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        public async Task<ResponseModel<OtpResponse>> InitiateOtp(long customerId, string phoneNumber, string trackingId,OtpPurpose purpose, string RecipientEmail=null)
        {
            var maintainTrackinId = false;
            if (!string.IsNullOrEmpty(trackingId))
            {
                var verificationDetail = await _context.Verifications.FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber && c.TrackingCode == trackingId);

                if (verificationDetail != null)
                {
                    if (verificationDetail.ExpiryDate > DateTime.Now)
                    {
                        maintainTrackinId = true;
                    }
                }
            }

            var previousVerification = await _context.Verifications.Where(c => c.CustomerId == customerId && c.PhoneNumber == phoneNumber && c.ExpiryDate > DateTime.Now).ToListAsync();
            if(previousVerification.Count()>0)
            {
                var prev = previousVerification.LastOrDefault();
                prev.ExpiryDate = DateTime.Now.AddMinutes(-2);
                prev.Status = OtpStatus.Expired;
                _context.Verifications.Update(prev);
            }

            Random generator = new Random();
            var otp = generator.Next(100001, 999999);
    
            var expiryDate = DateTime.Now.AddMinutes(_configuration.GetValue<int>("OTP:ExpiryMinutes"));
            var verification = new Verification
            {
                DateCreated = DateTime.Now,
                ExpiryDate = expiryDate,
                OtpCode = otp.ToString(),
                PhoneNumber = phoneNumber,
                CustomerId = customerId,
                Purpose = purpose,
                Status = Domain.Enum.OtpStatus.Initiated,
                TrackingCode = maintainTrackinId ? trackingId : Guid.NewGuid().ToString("N")
            };
            _context.Verifications.Add(verification);
            await _context.SaveChangesAsync();

            var response = new OtpResponse { OtpCode = verification.OtpCode, TrackingId = verification.TrackingCode };

            
            var customer = await _customerHelper.GetCustomer(customerId);
            if(customer.Email != null && customer.Email != "")
            {
                var subject = NotificationTypeEnum.Token;
                SendMailModel sendMailModel = new SendMailModel { Recipient = customer.Email };
                sendMailModel.AddSubject(subject);
                sendMailModel.AddMessage(subject, null, response.OtpCode, null);
                //sendMailModel.Recipient = customer.Email;
                sendMailModel.Recipient = customer.Email;
                sendMailModel.Template = new Dictionary<string, string>
                    {
                        { "customerName", $"{customer.FirstName} {customer.LastName}" },
                        { "messageContent", sendMailModel.Message },
                        { "senderName", "customer@firstbank.com"}
                    };

                await _notificationService.SendMail(sendMailModel);
            }
           

            return ResponseModel<OtpResponse>.Success(response, "Successful");
        }

        private async Task<bool> VerifyDetail(string trackingId, string otp, bool isTest)
        {
            if (isTest)
            {
                var testOTP = _configuration.GetValue<string>("OTP:testOTP");
                return otp == testOTP;
            }
            
            var verification = await _context.Verifications.OrderByDescending(d => d.Id).FirstOrDefaultAsync(c => c.TrackingCode == trackingId && c.OtpCode == otp);
            if (verification == null)
                return false;

            if (verification.Status != Domain.Enum.OtpStatus.Initiated)
                return false;

            verification.DateUsed = DateTime.Now;
            if (DateTime.Now >= verification.ExpiryDate)
            {
                verification.Status = Domain.Enum.OtpStatus.Expired;
                throw new CustomException("OTP has expired");
            }
            else
                verification.Status = Domain.Enum.OtpStatus.Applied;
            
            _context.Verifications.Update(verification);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> VerifyDetail(string trackingId, string otp, OtpPurpose otpPurpose)
        {
           
            var verification = await _context.Verifications.OrderByDescending(d => d.Id).FirstOrDefaultAsync(c => c.TrackingCode == trackingId && c.OtpCode == otp && c.Purpose == otpPurpose);
            if (verification == null)
                return false;

            if (verification.Status != Domain.Enum.OtpStatus.Initiated)
                return false;

            verification.DateUsed = DateTime.Now;
            if (DateTime.Now >= verification.ExpiryDate)
            {
                verification.Status = Domain.Enum.OtpStatus.Expired;
                throw new CustomException("OTP has expired");
            }
            else
                verification.Status = Domain.Enum.OtpStatus.Applied;

            _context.Verifications.Update(verification);
            await _context.SaveChangesAsync();
            return true;
        }
        private async Task<bool> VerifyDetail(long customerId, string otp, OtpPurpose otpPurpose)
        {

            var verification = await _context.Verifications.OrderByDescending(d => d.Id).FirstOrDefaultAsync(c => c.CustomerId == customerId && c.OtpCode == otp && c.Purpose == otpPurpose);
            if (verification == null)
                return false;

            if (verification.Status != Domain.Enum.OtpStatus.Initiated)
                return false;

            verification.DateUsed = DateTime.Now;
            if (DateTime.Now >= verification.ExpiryDate)
            {
                verification.Status = Domain.Enum.OtpStatus.Expired;
                throw new CustomException("OTP has expired");
            }
            else
                verification.Status = Domain.Enum.OtpStatus.Applied;

            _context.Verifications.Update(verification);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<ResponseModel> VerifyGeneralOtp(string trackingId, string otp)
        {
            var otpIsCorrect = await VerifyDetail(trackingId, otp, false);
            if (otpIsCorrect == false)
                throw new CustomException("OTP is invalid");
            return ResponseModel.Success("Successful..");
        }
        public async Task<ResponseModel> VerifyGeneralOtp(string trackingId, string otp, OtpPurpose otpPurpose )
        {
            var otpIsCorrect = await VerifyDetail(trackingId, otp, otpPurpose);
            if (otpIsCorrect == false)
                throw new CustomException("OTP is invalid");
            return ResponseModel.Success("Successful..");
        }
        public async Task<ResponseModel> VerifyGeneralOtp(long customerId, string otp, OtpPurpose otpPurpose)
        {
            var otpIsCorrect = await VerifyDetail( customerId, otp, otpPurpose);
            if (otpIsCorrect == false)
                throw new CustomException("OTP is invalid");
            return ResponseModel.Success("Successful..");
        }
        public async Task<ResponseModel<Token>> VerifyOtp(string trackingId, string otp )
        {
            var isTest = _configuration.GetValue<bool>("OTP:IsTest");
            var otpIsCorrect = false;
            if (isTest)
            {
                otpIsCorrect = await VerifyDetail(trackingId, otp, isTest);
                if (otpIsCorrect == false)
                    throw new CustomException("OTP is not correct");
                var customerDet = new Customer { Email = "test@d,com", Id = 1, FirstName = "Firstname" };
                var tk =  _customerHelper.GenerateToken(customerDet, JwtUserType.AnonynmousUser);
                return tk;
            }

            otpIsCorrect = await VerifyDetail(trackingId, otp, isTest=false);
            if (otpIsCorrect == false)
                throw new CustomException("OTP is invalid");
            var verification = await _context.Verifications.OrderByDescending(d => d.Id).FirstOrDefaultAsync(c => c.TrackingCode == trackingId && c.OtpCode == otp);
            var customerDetail = await _context.Customers.FirstOrDefaultAsync(c=>c.PhoneNumber.Contains(verification.PhoneNumber.formatPhoneNumber()));
            var token =  _customerHelper.GenerateToken(customerDetail, JwtUserType.AnonynmousUser);
        
            return token;
        }
    }
}
