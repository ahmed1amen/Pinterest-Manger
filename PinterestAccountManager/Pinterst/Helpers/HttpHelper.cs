using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Specialized;
using PinterestAccountManager.Pinterst.API;
using Newtonsoft.Json;

namespace PinterestAccountManager.Pinterst.Helpers
{
    class HttpHelper
    {

        public void AssginDefaultHeaders( ref HttpRequestMessage request, string CSRFT)
        {
            request.Headers.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            //request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.5");
            request.Headers.Add("DNT", "1");
            request.Headers.Add("Host", PinApiConstants.HOST);
            request.Headers.Add("Origin", PinApiConstants.URL_BASE);
            request.Headers.Add("Referer", PinApiConstants.URL_BASE);
            request.Headers.Add("User-Agent", PinApiConstants.UserAgent_Defualt);
            request.Headers.Add("X-APP-VERSION", "f9d1262");
            request.Headers.Add("X-CSRFToken", CSRFT);
            request.Headers.Add("X-NEW-APP", "1");
            request.Headers.Add("X-Pinterest-AppState", "active");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

        }

    public HttpRequestMessage GetDefaultRequest(HttpMethod method, Uri uri)
        {
           
            var request = new HttpRequestMessage(method, uri);

         
            return request;
        }

        public HttpRequestMessage GetDefaultRequest(HttpMethod method, Uri uri, Dictionary<string, string> data)
        {
            var request = GetDefaultRequest(HttpMethod.Post, uri);
            request.Content = new FormUrlEncodedContent(data);
            return request;
            
        }

    }
}
