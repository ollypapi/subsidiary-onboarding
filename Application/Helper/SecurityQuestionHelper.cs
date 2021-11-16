using Application.Common.Exceptions;
using Application.Common.Models;
using Application.Common.Models.Response;
using Application.Extentions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Helper
{
    public class SecurityQuestionHelper
    {
        private readonly IOnboardingDbContext _context;
        private readonly IConfiguration _configuration;
       
        public SecurityQuestionHelper(IOnboardingDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            
        }
        public async Task<ResponseModel> CreateSecurityQuestion(Customer customer, List<QuestionModel> questions)
        {
            /* var quest = await _context.SecurityQuestions.Where(c => c.CustomerId == customer.Id).ToListAsync();
             var remQuestion = questions.Select(d => d.Question.ToLower()).Except(quest.Select(c => c.Question.ToLower()));
             var quesionToLoad = questions.Where(c => remQuestion.Contains(c.Question)).ToList(); */

            var secQuestions = new List<SecurityQuestion>();
            questions.ForEach(c => secQuestions.Add(new SecurityQuestion { CustomerId = customer.Id, Question = c.Question, Answer = AppUtility.EnryptString(c.Answer) }));

            //quesionToLoad.ForEach(c => secQuestions.Add(new SecurityQuestion { CustomerId = customer.Id, Question = c.Question, Answer = AppUtility.EnryptString(c.Answer) }));
            customer.Stage = RegistrationStage.SecurityQuestionSet;
            _context.Customers.Update(customer);
            _context.SecurityQuestions.AddRange(secQuestions);

            await _context.SaveChangesAsync();
            return ResponseModel.Success("Successful..");
        }

        public async Task<ResponseModel> CreateSecurityQuestion(long customerId, List<QuestionModel> questions)
        {
            var secQuestions = new List<SecurityQuestion>();
            var customersSecurityQuestions = _context.SecurityQuestions.Where(s => s.CustomerId == customerId);
            if(customersSecurityQuestions.Count() > 0)
            {
                _context.SecurityQuestions.RemoveRange(customersSecurityQuestions);
            }
            questions.ForEach(c => secQuestions.Add(new SecurityQuestion { CustomerId = customerId, Question = c.Question, Answer = AppUtility.EnryptString(c.Answer) }));
            _context.SecurityQuestions.AddRange(secQuestions);
            await _context.SaveChangesAsync();
            return ResponseModel.Success("Successful..");
        }


        public async Task SetAccountStatus( string customerId, int stage)
        {

        }

        public async Task<int> DeleteSecurityQuestions( List<SecurityQuestion> questions)
        {
            _context.SecurityQuestions.RemoveRange(questions);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<SecurityQuestion>> GetSecurityQuestionByAccountNumber(string AccountNumber)
        {
            var customer = _context.Customers.Where(c => c.AccountNumber == AccountNumber).FirstOrDefault();
            if(customer == null)
                throw new CustomException("Unable to get user with the Supplied Account Number");
            var securityQuestions = _context.SecurityQuestions.Where(s => s.CustomerId == customer.Id);
            return securityQuestions.ToList();
        }

        public async Task<List<SecurityQuestion>> GetSecurityQuestionByAccountNumber(string AccountNumber, string CountryCode)
        {
            var customer = _context.Customers.Where(c => c.AccountNumber == AccountNumber && c.CountryId == CountryCode).FirstOrDefault();
            if (customer == null)
                throw new CustomException("Unable to get user with the Supplied Account Number");
            var securityQuestions = _context.SecurityQuestions.Where(s => s.CustomerId == customer.Id);
            return securityQuestions.ToList();
        }

        public async Task<SecurityQuestion> GetSecurityQuestion(long Id)
        {
            var securityQuestion = _context.SecurityQuestions.FirstOrDefault(s => s.Id == Id);
            if (securityQuestion == null)
                throw new CustomException("Security Question not Found");

            return securityQuestion;
        }

        public async Task<SecurityQuestion> VerifyQuestionAnswer(long Id, string Answer)
        {
            var securityQuestion = _context.SecurityQuestions.FirstOrDefault(s => s.Id == Id);
            if (securityQuestion == null)
                throw new CustomException("Security Question not Found");
            var QuestionAnswer = AppUtility.DecryptString(securityQuestion.Answer);
            if (QuestionAnswer.ToLowerInvariant().Equals(Answer.ToLowerInvariant()))
            {
                securityQuestion.Customer = _context.Customers.FirstOrDefault(c => c.Id == securityQuestion.CustomerId);
                return securityQuestion;
            }
                
            throw new CustomException("Wrong Answer");
        }
    }
}
