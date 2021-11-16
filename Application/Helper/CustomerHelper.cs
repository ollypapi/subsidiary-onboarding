using App.Commands;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Requests.FIBServiceRequests;
using Application.Common.Models.Response;
using Application.Common.Models.Response.FIBServiceResponse;
using Application.Extentions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class CustomerHelper
    {
        private readonly ILogger<CustomerHelper> _logger;
        private readonly IOnboardingDbContext _context;
        private readonly IFIService _fIService;
        private readonly TokenizationHelper _tokenizationHelper;
        private readonly ISubsidiaryPVC _subsidiaryPVC;
        private readonly ICryptoResource _cryptoResource;

        public CustomerHelper(ILogger<CustomerHelper> logger, IOnboardingDbContext context, IFIService fIService, 
            TokenizationHelper tokenizationHelper, ISubsidiaryPVC subsidiaryPVC, ICryptoResource cryptoResource)
        {
            _logger = logger;
            _context = context;
            _fIService = fIService;
            _tokenizationHelper = tokenizationHelper;
            _subsidiaryPVC = subsidiaryPVC;
            _cryptoResource = cryptoResource;
        }

        public async Task<bool> CustomerAlreadyExist(string reference)
        {
            return await _context.Customers.AnyAsync(c => c.Email.ToLower() == reference.ToLower() || c.PhoneNumber == reference);
        }

        public async Task<bool> CanAddMoreDevice(long customerId, string CountryCode)
        {

            var deviceSetting = await _context.Settings.FirstOrDefaultAsync(c => c.SettingType == Domain.Enum.SettingEnum.MaxDeviceCount && c.CountryCode == CountryCode);
            if (deviceSetting == null)
                throw new CustomException("Device Setting is not found");

            var currentDeviceCount = await _context.CustomerDevices.CountAsync(c => c.CustomerId == customerId && c.Status != DeviceStatus.Released.GetDescription());
            // var newDeviceCount = currentDeviceCount + 1;
            return currentDeviceCount < int.Parse(deviceSetting.Value);
            // return deviceSetting.Value > newDeviceCount;
        }

        public async Task<bool> CreateDevice(long customerId, DeviceModel device, string CountryCode)
        {
            var canAddMoreDevice = await CanAddMoreDevice(customerId, CountryCode);
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                throw new CustomException("Customer does not exist");
            var deviceDetail = await _context.CustomerDevices.FirstOrDefaultAsync(c => c.DeviceId == device.DeviceId && c.CustomerId == customerId);
            if (deviceDetail != null)
            {
                if (deviceDetail.Status == DeviceStatus.Activated.GetDescription() && deviceDetail.Status == DeviceStatus.Deactivated.GetDescription())
                    throw new CustomException("Device already registered for same user");

                deviceDetail.Status = DeviceStatus.Activated.GetDescription();
                deviceDetail.CustomerId = customerId;
                if (customer.DeviceId == null)
                {
                    customer.DeviceId = deviceDetail.DeviceId;
                    _context.Customers.Update(customer);
                }


                var deviceHistory = new DeviceHistory
                {
                    Description = DeviceHistoryEnum.Device_Enabled,
                };

                deviceDetail.DeviceHistories.Add(deviceHistory);

                _context.CustomerDevices.Update(deviceDetail);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                deviceDetail = new CustomerDevice
                {
                    CustomerId = customer.Id,
                    DeviceName = device.DeviceName,
                    Status = DeviceStatus.Activated.GetDescription(),
                    OS = device.OS,
                    DeviceId = device.DeviceId
                };
                var deviceHistory = new DeviceHistory
                {
                    Description = DeviceHistoryEnum.Device_Created
                };

                deviceDetail.DeviceHistories.Add(deviceHistory);
                _context.CustomerDevices.Add(deviceDetail);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ReferralCodeExist(string referralCode)
        {
            return await _context.Customers.AnyAsync(c => c.ReferralCode == referralCode);
        }

        public async Task<Customer> GetCustomer(long customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<Customer> GetCustomerByFICustomerId(string customerId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public async Task<Customer> GetCustomerFromPhone(string phoneNumber)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.PhoneNumber.Contains(phoneNumber.formatPhoneNumber()));
        }

        public async Task<List<Customer>> GetCustomerWithPendingDocuments(string countryId)
        {
            return await _context.Customers.Include(c=>c.Documents).Where(c=>c.CountryId == countryId && c.Documents.Any(d => d.Status == DocumentStatus.Pending)).OrderByDescending(c=>c.Id).ToListAsync();
        }

        public async Task<List<Customer>> GetCustomers(string countryId)
        {
            // c.CountryId == null is included to get data for test
            return await _context.Customers.Where(c => c.CountryId == null || c.CountryId == countryId).ToListAsync();
        }

        public async Task<Customer> GetCustomerByAccountNumber(string AccountNumber)
        {
            // c.CountryId == null is included to get data for test
            var c = await _context.Customers.FirstOrDefaultAsync(c => c.AccountNumber == AccountNumber);
            if (c == null)
                throw new CustomException("User not found");
            return c;

        }

        public async Task<Customer> GetCustomerByAccountNumberAndCountryCode(string AccountNumber, string CountryCode)
        {
            // c.CountryId == null is included to get data for test
            var c = await _context.Customers.FirstOrDefaultAsync(c => c.AccountNumber == AccountNumber && c.CountryId == CountryCode);
            if (c == null)
                throw new CustomException("User not found");
            return c;

        }

        public async Task<bool> IsCustomerExisting(string AccountNumber, string CountryCode)
        {
            // c.CountryId == null is included to get data for test
            var c = await _context.Customers.FirstOrDefaultAsync(c => c.AccountNumber == AccountNumber && c.CountryId == CountryCode);
            if (c == null)
                return false;
            return true;

        }

        public async Task<List<CustomerDevice>> GetCustomerDevices(long customerId)
        {
            // c.CountryId == null is included to get data for test
            return await _context.CustomerDevices.Where(c => c.Customer.CustomerId == customerId.ToString()).ToListAsync();
        }

        public async Task<CustomerDevice> GetCustomerDevice(string DeviceId, long CustomerId)
        {
            // c.CountryId == null is included to get data for test
            return await _context.CustomerDevices.FirstOrDefaultAsync(c => c.DeviceId == DeviceId && c.CustomerId == CustomerId);
        }

        public bool UpdateCustomerDevice(CustomerDevice Device)
        {
            // c.CountryId == null is included to get data for test
            _context.CustomerDevices.Update(Device);
            var deviceHistory = _context.DeviceHistories.FirstOrDefault(d => d.CustomerDeviceId == Device.Id);
            var customer = _context.Customers.FirstOrDefault(c => c.Id == Device.CustomerId);
            deviceHistory.Description = Device.Status == DeviceStatus.Activated.GetDescription() ? DeviceHistoryEnum.Device_Enabled : DeviceHistoryEnum.Device_Disabled;
            if (Device.Status != DeviceStatus.Released.GetDescription() && customer != null)
            {
                var customerActiveDevice = _context.CustomerDevices.FirstOrDefault(d => d.CustomerId == customer.Id && d.Status == DeviceStatus.Activated.GetDescription());
                if (customerActiveDevice != null)
                    customer.DeviceId = customerActiveDevice.DeviceId;
                else
                    customer.DeviceId = null;
                _context.Customers.Update(customer);
            }
            _context.DeviceHistories.Update(deviceHistory);
            _context.SaveChanges();
            return true;
        }

        public ResponseModel<Token> GenerateToken(Customer customer, JwtUserType jwtUserType)
        {

            var tokenData = new TokenDataModel
            {
                CustomerId = customer.Id,
                FirstName = customer.FirstName,
                JwtUserType = jwtUserType,
                LastName = customer.LastName,
                MobileNumber = customer.PhoneNumber,
                UserEmail = customer.Email,
                CIF = customer.CifId
            };

            var token = _tokenizationHelper.GetToken(tokenData);
            customer.RefreshToken = token.RefreshToken;
            customer.RefereshTokenExpiresIn = token.RefreshToken_ExpiresIn;
            _context.Customers.Update(customer);
            return ResponseModel<Token>.Success(token);

        }

        public async Task<ResponseModel> CreateCustomer(Customer customer)
        {
            if (await CustomerAlreadyExist(customer.PhoneNumber))
                throw new CustomException("Customer with similar Phone number already exists");

            if (customer.Email != null ? await CustomerAlreadyExist(customer.Email) : false)
                throw new CustomException("Duplicate Email address is not allowed..");
            var generator = new Random();
            var referralCode = generator.Next(10001, 99999);
            customer.ReferralCode = referralCode.ToString();
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return ResponseModel.Success("Successful");
        }

        public async Task<ResponseModel> UpdateCustomer(string accountNumber, Customer customer)
        {
            var customerDetail = await _context.Customers.FirstOrDefaultAsync(c => c.AccountNumber == accountNumber);
            if (customerDetail == null)
                throw new CustomException("Customer not found");
            customerDetail = customer;
            _context.Customers.Update(customerDetail);
            await _context.SaveChangesAsync();

            return ResponseModel.Success("Successful");
        }

        public async Task<ResponseModel> UpdateCustomer(Customer customer)
        {
            var customerDetail = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customer.Id);
            if (customerDetail == null)
                throw new CustomException("Customer not found");
            customerDetail = customer;
            _context.Customers.Update(customerDetail);
            await _context.SaveChangesAsync();

            return ResponseModel.Success("Successful");
        }

        public async Task<bool> VerifyCustomerDevice(string AccountNumber, string deviceid)
        {
            return await _context.CustomerDevices.AnyAsync(d => d.Customer.AccountNumber == AccountNumber && d.DeviceId == deviceid);
        }

        public async Task<ResponseModel> UpdateCustomerStage(long customerId, RegistrationStage stage)
        {
            var customerDetail = await _context.Customers.FindAsync(customerId);
            if (customerDetail == null)
                throw new CustomException("Customer not found");
            customerDetail.Stage = stage;
            _context.Customers.Update(customerDetail);
            await _context.SaveChangesAsync();

            return ResponseModel.Success("Successful");
        }

        public async Task<ResponseModel> SetTransactionPin(Customer customerDetail, string transactionPin)
        {

            byte[] transactionPinHash, transactionPinSalt;
            AppUtility.CreateHash(transactionPin, out transactionPinHash, out transactionPinSalt);
            customerDetail.TransactionPin = transactionPinHash;
            customerDetail.TransactionPinSalt = transactionPinSalt;
            customerDetail.Stage = RegistrationStage.PinSet;
            _context.Customers.Update(customerDetail);
            await _context.SaveChangesAsync();

            return ResponseModel.Success("Successful");
        }

        public async Task<ResponseModel> SetPassword(Customer customer, string password)
        {
            
            if (customer == null)
                throw new CustomException("Customer details not found");
            byte[] passwordHash, passwordSalt;
            AppUtility.CreateHash(password, out passwordHash, out passwordSalt);
            customer.PasswordHash = passwordHash;
            customer.PasswordSalt = passwordSalt;
            customer.Stage = RegistrationStage.PasswordSet;
            _context.Customers.Update(customer);
            var i = await _context.SaveChangesAsync();

            return ResponseModel.Success("Successful");
        }

        public async Task<ResponseModel> ConfirmCustomerDevice(string device, Customer customer)
        {
           /* var isCustomerDefaultDevice = customer.DeviceId.ToLower().Equals(device.ToLower());
            if (isCustomerDefaultDevice)
            {
                return ResponseModel.Success("Can Proceed");
            } */
            var deviceIsInCustomerList = await _context.CustomerDevices.AnyAsync(c => c.DeviceId != null && c.DeviceId.ToLower().Equals(device.ToLower()) && c.CustomerId == customer.Id && c.Status == DeviceStatus.Activated.GetDescription());
            if (deviceIsInCustomerList)
                return ResponseModel.Success("Can Proceed");
            return ResponseModel.Failure("Cannot Proceed");
        }

        public async Task<bool> isDeviceExisting(string deviceId)
        {
            return await _context.CustomerDevices.AnyAsync(d => d.DeviceId.ToLower().Equals(deviceId.ToLower()) && d.Status != DeviceStatus.Released.GetDescription());
        }

        public async Task<bool> isDeviceExisting(string deviceId, long customerId)
        {
            return await _context.CustomerDevices.AnyAsync(d => d.DeviceId.ToLower().Equals(deviceId.ToLower()) && d.Status != DeviceStatus.Released.GetDescription() && d.CustomerId !=customerId );
        }


        public async Task<ResponseModel<LoginResponse>> Login(string username, string password, string deviceId, string CountryId)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => (c.PhoneNumber == username || c.AccountNumber == username) && c.CountryId==CountryId);
            
            if (customer == null)
                throw new CustomException("Invalid credentials", ResponseCodeEnum.AuthenticationFailed.GetDescription());
           
            var documents = await _context.Documents.Where(d => d.CustomerId == customer.Id).ToListAsync();

            if (customer.IsLogin)
                throw new CustomException("Concurrent Login not allowed", ResponseCodeEnum.AuthenticationFailed.GetDescription());

            if (!(customer.Status == CustomerStatus.ACTIVATED.GetDescription()))
                throw new CustomException("Account Deactivated, Contact your Account Officer");

         /* if (customer.Stage != RegistrationStage.AccountCreated || customer.Stage != RegistrationStage.OnboardingCompleted)
                throw new CustomException("Cannot Login, Kindly Complete onbaording"); 
         */

            byte[] passwordHash, passwordSalt;
            var response = AppUtility.VerifyInformation(password, customer.PasswordSalt, customer.PasswordHash);
            if (!response)
            {
                await UpdateCustomerProfileStatus(customer.Id, customer.CountryId,false);
                throw new CustomException("Enter a valid credentials", ResponseCodeEnum.AuthenticationFailed.GetDescription());
            }
            
            var token = GenerateToken(customer, JwtUserType.RegisteredUser);

            var deviceAllowed = await ConfirmCustomerDevice(deviceId, customer);
            var isdeviceExisting = await isDeviceExisting(deviceId);

            if (!deviceAllowed.Status)
            {
                var canAddMoreDevice = await CanAddMoreDevice(customer.Id, customer.CountryId);
                var message = isdeviceExisting ? "Device Mismatch" : canAddMoreDevice ? "Device not registered. Please Add the device before login in " :
                    "You have reached the maximum number of device. Kindly release the device.";
                if (canAddMoreDevice)
                    throw new DeviceException(message, isdeviceExisting ? ResponseCodeEnum.DeviceMatchFailed.GetDescription() : canAddMoreDevice ? ResponseCodeEnum.AddDevice.GetDescription() : ResponseCodeEnum.DeviceValidationFailed.GetDescription(), customer.Id.ToString());
                throw new CustomException(message, isdeviceExisting ? ResponseCodeEnum.DeviceMatchFailed.GetDescription() : canAddMoreDevice ? ResponseCodeEnum.AddDevice.GetDescription() : ResponseCodeEnum.DeviceValidationFailed.GetDescription());
            }

            customer.LoginAttemptAccount = 0;
            await UpdateCustomerProfileStatus(customer.Id, customer.CountryId, true);
            LoginResponse res = new LoginResponse
            {
                Token = token.Data,
                User = new User
                {
                    cifId = "",
                    localBankCode = "",
                    photoUrl = "",
                    address = customer.Address,
                    dateOfBirth = customer.DOB.ToString(),
                    emailAddress = customer.Email,
                    firstName = customer.FirstName,
                    gender = customer.Gender,
                    hasUploadedKYC = documents.Count()>0 ? !documents.Any(d=>d.Status != DocumentStatus.Approved ):true,
                    lastLogin = DateTime.Now.ToString(),
                    lastName = customer.LastName,
                    middleName = customer.MiddleName,
                    phoneNumber = customer.PhoneNumber,
                    title = customer.Title,
                    customerId = customer.CustomerId,
                    accountNumber = customer.AccountNumber,
                    deviceId = deviceId
                }
            };
            return ResponseModel<LoginResponse>.Success(data: res, "Login Successful");
        }

        public async Task<bool> UpdateCustomerProfileStatus(long Id, string CountryCode, bool ValidCredential)
        {
            var PasswordRetryMaxCount = await _context.Settings.Where(c => c.SettingType == SettingEnum.PasswordRetryMaxCount && c.CountryCode == CountryCode).FirstOrDefaultAsync();
            var customer = await _context.Customers.FindAsync(Id);
            if(PasswordRetryMaxCount!= null && customer != null)
            {
                if (customer.Status == CustomerStatus.ACTIVATED.GetDescription() && customer.LoginAttemptAccount < int.Parse(PasswordRetryMaxCount.Value))
                    customer.LoginAttemptAccount = ValidCredential ? 0: customer.LoginAttemptAccount + 1;
                else
                if (customer.Status == CustomerStatus.ACTIVATED.GetDescription() && customer.LoginAttemptAccount == int.Parse(PasswordRetryMaxCount.Value))
                    customer.Status = CustomerStatus.DEACTIVATED.GetDescription();
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
            }
            return customer.Status == CustomerStatus.ACTIVATED.GetDescription();
        }

        public async Task<ResponseModel<LoginResponse>> RefreshAuthToken(string refreshToken )
        {
            var customer = await _context.Customers.Where(c => c.RefreshToken == refreshToken).FirstOrDefaultAsync();
            if (customer == null)
                throw new CustomException("Invalid Token", ResponseCodeEnum.AuthenticationFailed.GetDescription());

            if (customer.RefereshTokenExpiresIn < DateTime.Now)
            {
                customer.IsLogin = false;
                _context.Customers.Update(customer);
                _context.SaveChanges();
                throw new CustomException("token already expired");
            }

            var token = GenerateToken(customer, JwtUserType.RegisteredUser);
            LoginResponse res = new LoginResponse
            {
                Token = token.Data,
                User = new User
                {
                    cifId = "",
                    localBankCode = "",
                    photoUrl = "",
                    address = customer.Address,
                    dateOfBirth = customer.DOB.ToString(),
                    emailAddress = customer.Email,
                    firstName = customer.FirstName,
                    gender = customer.Gender,
                    hasUploadedKYC = false,
                    lastLogin = DateTime.Now.ToString(),
                    lastName = customer.LastName,
                    middleName = customer.MiddleName,
                    phoneNumber = customer.PhoneNumber,
                    title = customer.Title,
                    customerId = customer.CustomerId,
                    accountNumber = customer.AccountNumber
                }
            };
            return ResponseModel<LoginResponse>.Success(data: res);
        }

        public async Task<ResponseModel> Logout(long customerId)
        {
            var customers =  _context.Customers.Where(c => c.Id == customerId || c.CustomerId == customerId.ToString());
            if(!customers.Any())
                throw new CustomException("User does not Exist", ResponseCodeEnum.AuthenticationFailed.GetDescription());
            var customer = await customers.FirstOrDefaultAsync();
            customer.IsLogin = false;
            customer.LoginAttemptAccount = 0;
            customer.RefereshTokenExpiresIn = DateTime.Now.AddMinutes(2);
            customer.RefreshToken = "";
            _context.Customers.Update(customer);
            _context.SaveChanges();
            return ResponseModel.Success("Logout Successfully");
           // customer.RefereshTokenExpiresIn = null;
        }

        public async Task<ResponseModel> VerifyPin(string CustomerId, string Pin)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == CustomerId);
            if (customer == null)
                throw new CustomException("Invalid Pin");
            var response = AppUtility.VerifyInformation(Pin, customer.TransactionPinSalt, customer.TransactionPin);
            if (response)
                return ResponseModel.Success();
            throw new CustomException("Invalid Pin");
        }

        public async Task<ResponseModel<CustomerAccountResponse>> OnboardExistingCustomer(string mobileNo, string accountNo, string countryCode)
        {
            await validateSchemeCode(accountNo, countryCode);

            var customerExist = await IsCustomerExisting(accountNo,countryCode);
            if (customerExist)
            {
                var customer = await GetCustomerByAccountNumberAndCountryCode(accountNo,countryCode);
                if(customer.Stage == RegistrationStage.OnboardingCompleted)
                    throw new CustomException("Customer already exist, Kindly login");
                var data = new CustomerAccountResponse { 
                     Address = customer.Address, CifId = customer.CifId, Stage = customer.Stage.GetDescription(),Country= customer.CountryId, CustomerId = customer.CustomerId,DateOfBirth = customer.DOB, Email = customer.Email,FirstName = customer.FirstName,Gender = customer.Gender,LastName = customer.LastName,MiddleName = customer.MiddleName, MobileNo = customer.PhoneNumber,State = customer.State,Title = customer.Title, AccountNumber= customer.AccountNumber,IsExistingCustomer = true
                };
                return ResponseModel<CustomerAccountResponse>.Success(data);
            }
               
            _logger.LogInformation($"REQUEST ACCOUNT DETAIL =====> {accountNo}, MOBILE NUMBER =====> {mobileNo}");
            var resp = await _fIService.GetCustomerByAccountNo(countryCode, accountNo);

            await validateSchemeCode(accountNo, countryCode);

            _logger.LogInformation($"REQUEST ACCOUNT DETAIL =====> {accountNo}, MOBILE NUMBER =====> {mobileNo}  {JsonConvert.SerializeObject(resp)}");
            if (resp.ResponseCode.Equals("00") && resp.MobileNo != null && resp.MobileNo.formatPhoneNumber().Equals(mobileNo.formatPhoneNumber()))
            {
                return ResponseModel<CustomerAccountResponse>.Success(resp);
            }

           
            if (resp.ResponseCode.Equals("00") && ( resp.MobileNo == null || (resp.MobileNo != null && !resp.MobileNo.formatPhoneNumber().Equals(mobileNo.formatPhoneNumber()))))
                throw new CustomException("Invalid Phone Number");

            _logger.LogError($"REQUEST ACCOUNT DETAIL Failed =====> {accountNo}, MOBILE NUMBER =====> {mobileNo}");
            throw new CustomException(resp.ResponseMessage);
            
        }

        public async Task validateSchemeCode(string AccountNumber, string CountryCode)
        {
            var accountDetails = await _fIService.GetAccountDetails(CountryCode, AccountNumber);
            if(accountDetails.ResponseCode != "00" )
                throw new CustomException($"Invalid account number");
            var schemeCode =  _context.SchemeCodes.Where(sc => sc.CountryCode == CountryCode);
            if (schemeCode.Count() > 0 && !schemeCode.Any(c => c.Code == accountDetails.ProductCode))
                  throw new CustomException($"{accountDetails.Product} is not permiited");
        }

        public async Task<ResponseModel<CustomerAccountResponse>> OnboardExistingCustomerWithCard(CustomerCardValidationRequest req)
        {
            var validateUser = await ValidateUserDetails(req.CountryId, req.MobileNumber, req.AccountNumber);
            if (validateUser.Equals("00"))
            {
                var customerExist = await IsCustomerExisting(req.AccountNumber, req.CountryId);
                if (customerExist)
                {
                    var customer = await GetCustomerByAccountNumberAndCountryCode(req.AccountNumber, req.CountryId);
                    if (customer.Stage.Equals(RegistrationStage.OnboardingCompleted))
                        throw new CustomException("Customer already exist, Kindly login");
                    var data = new CustomerAccountResponse
                    {
                        Address = customer.Address,
                        CifId = customer.CifId,
                        Stage = customer.Stage.GetDescription(),
                        Country = customer.CountryId,
                        CustomerId = customer.CustomerId,
                        DateOfBirth = customer.DOB,
                        Email = customer.Email,
                        FirstName = customer.FirstName,
                        Gender = customer.Gender,
                        LastName = customer.LastName,
                        MiddleName = customer.MiddleName,
                        MobileNo = customer.PhoneNumber,
                        State = customer.State,
                        Title = customer.Title,
                        AccountNumber = customer.AccountNumber
                    };
                    return ResponseModel<CustomerAccountResponse>.Success(data);

                }
                _logger.LogInformation($"REQUEST ACCOUNT DETAIL =====> {req.AccountNumber}, MOBILE NUMBER =====> {req.MobileNumber}");
                var resp = await _subsidiaryPVC.VerifyCardPIN(new CustomerCardValidationRequest
                {
                    AccountNumber = req.AccountNumber,
                    MobileNumber = req.MobileNumber,
                    CountryId = req.CountryId,
                    CardPan = _cryptoResource.MaskCardPan(req.CardPan),
                    Pin = _cryptoResource.Encrypt(req.Pin)
                });
                _logger.LogInformation($"REQUEST ACCOUNT DETAIL =====> {resp.AccountNumber}, MOBILE NUMBER =====> {resp.MobileNo}  {JsonConvert.SerializeObject(resp)}");
                if (resp.ResponseCode.Equals("00"))
                    return ResponseModel<CustomerAccountResponse>.Success(resp);

                _logger.LogError($"REQUEST ACCOUNT DETAIL Failed =====> {resp.AccountNumber}, MOBILE NUMBER =====> {resp.MobileNo}");
                throw new CustomException(resp.ResponseMessage);
            }

            throw new CustomException("Unable to locate record");
        }

        private async Task<string> ValidateUserDetails(string countryId, string mobileNumber, string accNo)
        {
            var verifyPhone = await _fIService.GetCustomerByMobileNo(countryId, mobileNumber);
            var verifyAccount = await _fIService.GetCustomerByAccountNo(countryId, accNo);
            if (!verifyPhone.ResponseCode.Equals("00"))
                throw new CustomException("Phone number does not exist.");

            if (verifyAccount.ResponseCode.Equals("00"))
            {
                var custPhoneNumber = verifyAccount.MobileNo;
                var slipNumber = mobileNumber.StartsWith('0');
                string newNumber = string.Empty;
                string trucNumber = string.Empty;
                if (slipNumber)
                {
                    newNumber = mobileNumber.Substring(1, mobileNumber.Length - 1);
                }
                else
                    newNumber = mobileNumber;

                trucNumber = custPhoneNumber.Substring(3, custPhoneNumber.Length - 3);
                if (!trucNumber.Equals(newNumber))
                    throw new CustomException("Phone number is not linked to the account number");
            }
            else
                throw new CustomException("Account number does not exist.");

            return "00";
        }

    }
}
