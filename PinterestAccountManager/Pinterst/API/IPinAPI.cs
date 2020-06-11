using PinterestAccountManager.Pinterst.API.Processors;
using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.API
{
   public interface IPinApi
   

    {


        /// <summary>
        ///     Current <see cref="IHttpRequestProcessor"/>
        /// </summary>
        IHttpRequestProcessor HttpRequestProcessor { get; }
        /// <summary>
        ///     Current HttpClient
        /// </summary>
        HttpClient HttpClient { get; }
        /// <summary>
        ///     Indicates whether user authenticated or not
        /// </summary>
        bool IsUserAuthenticated { get; }
        /// <summary>
        ///     Live api functions.
        /// </summary>
        IPinProcessor PinProcessor { get; }

        /// <summary>
        ///     Live api functions.
        /// </summary>
        IUserProcessor UserProcessor { get; }

        Task<IResult<PinLoginResult>> LoginAsync(bool isNewLogin = true);




        #region State data

        /// <summary>
        ///     Get current state info as Memory stream
        /// </summary>
        /// <returns>State data</returns>
        Stream GetStateDataAsStream();
        /// <summary>
        ///     Get current state info as Json string
        /// </summary>
        /// <returns>State data</returns>
        string GetStateDataAsString();
        /// <summary>
        ///     Get current state info as Json string asynchronously
        /// </summary>
        /// <returns>
        ///     State data
        /// </returns>
        /// 

        ///<summary>
        ///     Get current state as StateData object
        /// </summary>
        /// <returns>
        ///     State data object
        /// </returns>
        StateData GetStateDataAsObject();

        Task<string> GetStateDataAsStringAsync();
        /// <summary>
        ///     Get current state info as Memory stream asynchronously
        /// </summary>
        /// <returns>State data</returns>
        Task<Stream> GetStateDataAsStreamAsync();
        /// <summary>
        ///     Set state data from provided stream
        /// </summary>
        void LoadStateDataFromStream(Stream data);
        /// <summary>
        ///     Set state data from provided json string
        /// </summary>
        void LoadStateDataFromString(string data);
        /// <summary>
        ///     Set state data from provided stream asynchronously
        /// </summary>

        /// <summary>
        ///     Set state data from object
        /// </summary>
        void LoadStateDataFromObject(StateData stateData);

        Task LoadStateDataFromStreamAsync(Stream stream);
        /// <summary>
        ///     Set state data from provided json string asynchronously
        /// </summary>
        Task LoadStateDataFromStringAsync(string json);


        #endregion State data

    }
}

