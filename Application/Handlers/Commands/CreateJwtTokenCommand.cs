using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.ComponentModel;
using Application.Extentions;
using Application.Common.Models.Response;
using Application.Common.Models;
using Application.Helper;

namespace App.Commands
{
    public class CreateJwtTokenCommand : TokenDataModel, IRequest<ResponseModel<Token>>
    {


       
    }


    public class CreateJwtTokenCommandHandler : IRequestHandler<CreateJwtTokenCommand, ResponseModel<Token>>
    {

        private readonly ILogger<CreateJwtTokenCommandHandler> _logger;
        private readonly TokenizationHelper _tokenizationHelper;


        public CreateJwtTokenCommandHandler(
               IConfiguration config,
             ILogger<CreateJwtTokenCommandHandler> logger)
        {

            _logger = logger;
        }

        public async Task<ResponseModel<Token>> Handle(CreateJwtTokenCommand request, CancellationToken cancellationToken)
        {
            TokenDataModel tokenData = new TokenDataModel {
                CIF = request.CIF,
                CustomerId = request.CustomerId,
                FirstName = request.FirstName,
                JwtUserType = request.JwtUserType,
                LastName = request.LastName,
                MobileNumber = request.MobileNumber,
                UserEmail = request.UserEmail,
                UserName = request.UserName
            };
            var token = _tokenizationHelper.GetToken(tokenData);
            return ResponseModel<Token>.Success(token);
        }


    }



}
