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
    public interface IPinApiBuilder
    {

        /// <summary>
        ///     Create new API instance
        /// </summary>
        /// <returns>API instance</returns>
        IPinApi Build();

        /// <summary>
        ///     Set specific HttpClient
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        /// <returns>API Builder</returns>
        IPinApiBuilder   UseHttpClient(HttpClient httpClient);

        /// <summary>
        ///     Set custom HttpClientHandler to be able to use certain features, e.g Proxy and so on
        /// </summary>
        /// <param name="handler">HttpClientHandler</param>
        /// <returns>API Builder</returns>
        IPinApiBuilder UseHttpClientHandler(HttpClientHandler handler);


        /// <summary>
        ///     Specify user login, password from here
        /// </summary>
        /// <param name="user">User auth data</param>
        /// <returns>API Build
        IPinApiBuilder SetUser(UserSessionData user);


        /// <summary>
        ///     Use custom logger
        /// </summary>
        /// <param name="logger">IInstaLogger implementation</param>
        /// <returns>API Builder</returns>
        IPinApiBuilder UseLogger(IPinLogger logger);






        /// <summary>
        ///     Set delay between requests. Useful when API supposed to be used for mass-bombing.
        /// </summary>
        /// <param name="delay">Timespan delay</param>
        /// <returns>API Builder</returns>
        IPinApiBuilder SetRequestDelay(IRequestDelay delay);


    }
}
