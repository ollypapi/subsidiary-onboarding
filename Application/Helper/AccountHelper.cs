using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.AccountService;
using Application.Extensions;
using Application.Extentions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class AccountHelper
    {
        private readonly IAccountService _accountService;
        private readonly IFIService _fIService;
        private readonly DocumentHelper _documentHelper;
        private readonly CustomerHelper _customerHelper;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public AccountHelper(IConfiguration configuration, IAccountService accountService, DocumentHelper documentHelper, CustomerHelper customerHelper, IOnboardingDbContext context, IFIService fIService, INotificationService notificationService)
        {
            _accountService = accountService;
            _documentHelper = documentHelper;
            _customerHelper = customerHelper;
            _configuration = configuration;
            _fIService = fIService;
            _notificationService = notificationService;
        }
        public async Task<ResponseModel<AccountCreationResponse>> CreateCustomerAccount(string phoneNumber, string countryId)
        {
            var customer = await _customerHelper.GetCustomerFromPhone(phoneNumber);
            if (customer == null)
                throw new CustomException("Customer does not exist");

            // check if customer already have an Account no, create a new account
            if (customer.AccountNumber == null)
            {
                var resp = await CreateAccount(customer, countryId);
                string accountNumber = resp.AccountNumber;
                var accountDetail = await _accountService.GetAccountDetail(new AccountDetailRequest { AccountNumber = accountNumber, CountryId = countryId });
                customer.AccountNumber = resp.AccountNumber;
                customer.CifId = accountDetail.CifId;
                customer.CustomerId = accountDetail.CustomerId;

                SendMailModel sendMailModel = new SendMailModel { Recipient = customer.Email };
                sendMailModel.AddSubject(NotificationTypeEnum.AccountCreated);
                sendMailModel.AddMessage(NotificationTypeEnum.AccountCreated, customer.AccountNumber, null, null);
                sendMailModel.Recipient = customer.Email;
                sendMailModel.Template = new Dictionary<string, string>
                    {
                        { "customerName", $"{customer.FirstName} {customer.LastName}" },
                        { "messageContent", sendMailModel.Message },
                        { "senderName", sendMailModel.Subject }
                    };

                await _notificationService.SendMail(sendMailModel);
            }
            customer.Stage = RegistrationStage.OnboardingCompleted;
            var r = await _customerHelper.UpdateCustomer(customer);
            return ResponseModel<AccountCreationResponse>.Success(new AccountCreationResponse { AccountNumber = customer.AccountNumber, ResponseCode="00",ResponseMessage="Onboarding was successfull" });
        }


        public async Task<AccountCreationResponse> CreateAccount(Customer customer, string countryId)
        {
            var documentTask = _documentHelper.GetCustomerDocuments(customer.Id);
            var subsidiariesTask = _fIService.GetAllSusidiaries("01");
            var occupationsTask = _fIService.GetOccupations(countryId);
            var countriesTask = _fIService.GetCountriess(countryId);

            var tasks = new List<Task>();
            tasks.Add(documentTask);
            tasks.Add(subsidiariesTask);
            tasks.Add(occupationsTask);
            tasks.Add(countriesTask);
            await Task.WhenAll(tasks);
            var documents = await documentTask;
            var subsidiariesResponse = await subsidiariesTask;
            var occupationResp = await occupationsTask;
            var countriesResp = await countriesTask;
            //  var occupation = occupationResp.Occupations.FirstOrDefault(c=>c.Value.ToLower().Trim().Contains("others".ToLower().Trim()));
            var occupation = occupationResp.Occupations == null || occupationResp.Occupations.Count == 0 ?
                countryId == "05" ? "010" : countryId == "06" ? "011" :  "999" : occupationResp.Occupations.FirstOrDefault(c => c.Value.ToLower().Trim().Contains("others".ToLower().Trim())).Id; 
            if (subsidiariesResponse == null || subsidiariesResponse.Subsidiaries == null)
                throw new CustomException("Unable to get Currency code");
            if (documents == null || documents.Count == 0)
                throw new CustomException("Unable to get uploaded document");
            var subsidiary = subsidiariesResponse.Subsidiaries.FirstOrDefault(s => s.CountryId == countryId);
            //var Country = countriesResp.Countries.FirstOrDefault(c => c.Value.ToLower().Trim().Contains(subsidiary.Name.ToLower().Trim()));
          //  if (Country == null)
          //      throw new CustomException("Unable to get Customer Country");
            var idImage = documents.FirstOrDefault(c => c.Identification.HasValue);
            if (idImage == null || idImage.DocumentUrl == null)
                throw new CustomException("User ID image cannot be found");
             var selfie = documents.FirstOrDefault(c => c.DocumentType == Domain.Enum.DocumentType.Selfie);
           // var selfie = documents[1];
            if (selfie == null || selfie.DocumentUrl == null)
                throw new CustomException("Selfie image cannot be found");
             var signature = documents.FirstOrDefault(c => c.DocumentType == Domain.Enum.DocumentType.Signature);
           // var signature = documents[1];
            if (signature == null || signature.DocumentUrl == null)
                throw new CustomException("Signature image cannot be found");
            //var imageBaseUrl = _configuration.GetValue<string>("Util:FileManagerBase");

            var idImage64 = _documentHelper.GetBase64Image($"files/{idImage.DocumentUrl}");
            var selfie64 = _documentHelper.GetBase64Image($"files/{selfie.DocumentUrl}");
            var signature64 = _documentHelper.GetBase64Image($"files/{signature.DocumentUrl}");

            var account = new AccountCreationRequest
            {
                AccountType = AccountTypeEnum.Saving.GetDescription(),
                Address = customer.Address,
                BranchCode = customer.Branch,
                 City = customer.Town,
               // BranchCode = "303",
                // City = "35",
                Country = subsidiary.CountryId,
                // Country = "GIN",
                //  Country = countriesResp.Countries.FirstOrDefault( c=>c.Value.ToLowerInvariant() == subsidiary.Name.ToLowerInvariant()).Id,
                // Occupation = occupation.Id,
                // Occupation = "999",
                // Occupation = occupation.Id,
                Occupation = occupation,
                CountryId = countryId,
                CurrenyCode = subsidiary.CurrencyCode,
                DateOfBirth = customer.DOB,
                Email = customer.Email,
                FirstName = customer.FirstName,
                Gender = customer.Gender,
                HouseNumber = "00",
                IdImage = idImage64,
                IdNumber = idImage.IdentificationNumber,
                //IdType = idImage.Identification.ToString(),
                IdType = customer.IdType,
                LastName = customer.LastName,
                MaritalStatus = customer.MaritalStatus,
               // MaritalStatus = "VEUF",
                MiddleName = customer.MiddleName,
                MobileNumber = customer.PhoneNumber,
               // MobileNumber = "22430315551",
                Nationality = customer.Nationality,
               // Nationality = "GIN",
                PassportPhoto = selfie64,
                PostalCode = "",
                ReferralCode = customer.ReferralCode,

               // ReferralCode = "00211",
                // Region = customer.Region,
                Region =null,
                Salutation = customer.Title,
               // Salutation= "MS.",
                Signature = signature64,
                State = customer.State,
                // State="08",
                StreetName = customer.Address
            };

            account.CurrenyCode = subsidiariesResponse.Subsidiaries.FirstOrDefault(s => s.CountryId == countryId).CurrencyCode;
            string payload = JsonConvert.SerializeObject(account);
            var accountCreationResponse = await _accountService.CreateAccount(account);
            string response = JsonConvert.SerializeObject(accountCreationResponse);
            if (accountCreationResponse == null || accountCreationResponse.ResponseCode != "00")
                throw new CustomException("Unable to create Account");
            return accountCreationResponse;
        }


        public async Task SendMail(string AccountNumber, string AccountName)
        {
            var MailModel = new SendMailModel {
                Message = $"{AccountName} just created an Account awaiting your Approval ",
            };
        }
    }
}
