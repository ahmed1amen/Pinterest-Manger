using PinterestAccountManager.Pinterst.API;
using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst
{
    class PinApiBuilder : IPinApiBuilder
    {
        private IRequestDelay _delay = RequestDelay.Empty();

        private HttpClient _httpClient;
        private HttpClientHandler _httpHandler = new HttpClientHandler();
        private IHttpRequestProcessor _httpRequestProcessor;
        private IPinLogger _logger;
        private ApiRequestMessage _requestMessage;
        private UserSessionData _user;



        private PinApiBuilder()
        {
        }


        public IPinApi Build()
        {


            _requestMessage = new ApiRequestMessage
            {

                password = _user?.Password,
                username_or_email = _user?.UserName,

            };


            if (_user == null)
                _user = UserSessionData.Empty;

            if (_httpHandler == null) _httpHandler = new HttpClientHandler();

            if (_httpClient == null)
                _httpClient = new HttpClient(_httpHandler) { BaseAddress = new Uri(PinApiConstants.URL_BASE) };


            if (_httpRequestProcessor == null)
                _httpRequestProcessor =
                    new HttpRequestProcessor(_delay, _httpClient, _httpHandler, _requestMessage, _logger);

          

            var instaApi = new PinApi(_user, _logger, _httpRequestProcessor);

            return instaApi;



        }

        public IPinApiBuilder UseHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            return this;
        }

        public IPinApiBuilder UseHttpClientHandler(HttpClientHandler handler)
        {
            _httpHandler = handler;
            return this;
        }


        public static IPinApiBuilder CreateBuilder()
        {
            return new PinApiBuilder();
        }
        public IPinApiBuilder SetUser(UserSessionData user)
        {
            _user = user;
            return this;
        }

        public IPinApiBuilder UseLogger(IPinLogger logger)
        {
            _logger = logger;
            return this;
        }

        public IPinApiBuilder SetRequestDelay(IRequestDelay delay)
        {
            if (delay == null)
                delay = RequestDelay.Empty();
            _delay = delay;
            return this;
        }
    }
}
