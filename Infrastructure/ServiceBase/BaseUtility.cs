using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceBase
{
    public class BaseUtility
    {
        private readonly IConfiguration _configuration;
        public BaseUtility(IConfiguration  configuration)
        {
            _configuration = configuration;
        }
      
        public string BaseUrl
        {
            get
            {
                var baseUrl = _configuration.GetValue<string>("FISetting:BaseUrl");
                return baseUrl;
            }
        }
        public string AppId
        {
            get
            {
                var baseUrl = _configuration.GetValue<string>("FISetting:AppId");
                return baseUrl;
            }
        }
        public string AppKey
        {
            get
            {
                var baseUrl = _configuration.GetValue<string>("FISetting:AppKey");
                return baseUrl;
            }
        }

        public string EmailServiceBaseUrl
        {
            get
            {
                var baseUrl = _configuration.GetValue<string>("EmailNotification:EmailBaseUrl");
                return baseUrl;
            }
        }

        public string EmailServiceAppKey
        {
            get
            {
                var baseUrl = _configuration.GetValue<string>("EmailNotification:AppKey");
                return baseUrl;
            }
        }

        public string EncryptionKey
        {
            get
            {
                var key = _configuration.GetValue<string>("Encryption:key");
                return key;
            }

        }



    }
}
