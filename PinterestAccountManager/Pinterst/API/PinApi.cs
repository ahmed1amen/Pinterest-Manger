using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinterestAccountManager.Pinterst.API.Processors;
using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Enums;
using PinterestAccountManager.Pinterst.Helpers;
using PinterestAccountManager.Pinterst.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.API
{
    internal class PinApi : IPinApi
    {


                    

        #region Variables and properties

        private IRequestDelay _delay = RequestDelay.Empty();
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        HttpClientHandler HttpHandler { get; set; }
        ApiRequestMessage RequestMessage { get; }
        HttpClient Client { get; }
        private readonly IPinLogger _logger;
        private UserSessionData _userSession;
        private HttpHelper _httpHelper { get; set; }
        private UserAuthValidate _userAuthValidate;

        private UserSessionData _user
        {
            get { return _userSession; }
            set { _userSession = value; _userAuthValidate.User = value; }
        }
        private bool _isUserAuthenticated;
        /// <summary>
        ///     Indicates whether user authenticated or not
        /// </summary>
        public bool IsUserAuthenticated
        {
            get { return _isUserAuthenticated; }
            internal set { _isUserAuthenticated = value; _userAuthValidate.IsUserAuthenticated = value; }
        }
        /// <summary>
        ///     Current HttpClient
        /// </summary>
        public HttpClient HttpClient { get => _httpRequestProcessor.Client; }
        /// <summary>
        ///     Current <see cref="IHttpRequestProcessor"/>
        /// </summary>
        public IHttpRequestProcessor HttpRequestProcessor => _httpRequestProcessor;


        #endregion Variables and properties
        #region SessionHandler
      //  private ISessionHandler _sessionHandler;
        //public ISessionHandler SessionHandler { get => _sessionHandler; set => _sessionHandler = value; }
        #endregion

        #region Processors

        private IPinProcessor _pinProcessor;
        private IUserProcessor _userProcessor;





        /// <summary>
        ///     Collection api functions.
        /// </summary>
        public IPinProcessor PinProcessor => _pinProcessor;



        /// <summary>
        ///     Collection User api functions.
        /// </summary>
        public IUserProcessor UserProcessor => _userProcessor;
        #endregion Processors


        #region Constructor

        public PinApi(UserSessionData user, IPinLogger logger,  IHttpRequestProcessor httpRequestProcessor)
        {
            _userAuthValidate = new UserAuthValidate();
            _user = user;
            _logger = logger;
            
            _httpRequestProcessor = httpRequestProcessor;
            
            _httpHelper = new HttpHelper();
        }


        #endregion Constructor



        /// <summary>
        ///     Login using given credentials asynchronously
        /// </summary>
        /// <param name="isNewLogin"></param>
        /// <returns>
        ///     Success --> is succeed
        ///     TwoFactorRequired --> requires 2FA login.
        ///     BadPassword --> Password is wrong
        ///     InvalidUser --> User/phone number is wrong
        ///     Exception --> Something wrong happened
        ///     ChallengeRequired --> You need to pass Instagram challenge
        /// </returns>
        public async Task<IResult<PinLoginResult>> LoginAsync(bool isNewLogin = true)
        {
            try
            {
                bool needsRelogin = false;
                ReloginLabel:
                if (isNewLogin)
                {


                    
                    var firstrequest = _httpHelper.GetDefaultRequest(HttpMethod.Get, new Uri(PinApiConstants.URL_BASE));
                    _httpHelper.AssginDefaultHeaders(ref firstrequest, "1234");
                    _httpRequestProcessor.HttpHandler.CookieContainer.Add(new Uri(PinApiConstants.URL_BASE), new Cookie("csrftoken", "1234"));
                    var firstresponse = await _httpRequestProcessor.SendAsync(firstrequest);
                    var html = await firstresponse.Content.ReadAsStringAsync();
                    _logger?.LogResponse(firstresponse);

                }
                
               

                var cookies =
                    _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client
                        .BaseAddress);

                var csrftoken = cookies[PinApiConstants.CSRFTOKEN]?.Value ?? string.Empty;
                _user.CsrfToken = csrftoken;
                var instaUri = PinApiConstants.RESOURCE_LOGIN;
                var data = string.Empty;


                if (isNewLogin)
                    data = "{\"options\":" + _httpRequestProcessor.RequestMessage.GetMessageString()+ ",\"context\":{}}";


                var fields = new Dictionary<string, string>
                {
                    {"source_url", ""},
                    {"data",data}
                };
                var request = _httpHelper.GetDefaultRequest(HttpMethod.Post, UriCreator.GetLoginUri(), fields);
                _httpHelper.AssginDefaultHeaders(ref request, "1234");
                var response = await _httpRequestProcessor.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();


                var userinfo = await UserProcessor.GetCurrentUserAsync();





                cookies =  _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);

                _user.UserName = userinfo.Value.username;
                _user.CsrfToken = cookies[PinApiConstants.CSRFTOKEN]?.Value ?? string.Empty;




                InvalidateProcessors();

                /*
                 * ============================================== LOGIN Complete =================================================
                 */



                //   if (response.StatusCode != HttpStatusCode.OK)
                //    {
                //var loginFailReason = JsonConvert.DeserializeObject<PinLoginBaseResponse>(json);

                //if (loginFailReason.InvalidCredentials)
                //    return Result.Fail("Invalid Credentials",
                //        loginFailReason.ErrorType == "bad_password"
                //            ? PinLoginResult.BadPassword
                //            : PinLoginResult.InvalidUser);
                //if (loginFailReason.TwoFactorRequired)
                //{
                //    if (loginFailReason.TwoFactorLoginInfo != null)
                //        _httpRequestProcessor.RequestMessage.Username = loginFailReason.TwoFactorLoginInfo.Username;
                //    _twoFactorInfo = loginFailReason.TwoFactorLoginInfo;
                //    //2FA is required!
                //    return Result.Fail("Two Factor Authentication is required", PinLoginResult.TwoFactorRequired);
                //}
                //if (loginFailReason.ErrorType == "checkpoint_challenge_required"
                //   /* || !string.IsNullOrEmpty(loginFailReason.Message) && loginFailReason.Message == "challenge_required"*/)
                //{
                //    _challengeinfo = loginFailReason.Challenge;

                //    return Result.Fail("Challenge is required", PinLoginResult.ChallengeRequired);
                //}
                //if (loginFailReason.ErrorType == "rate_limit_error")
                //{
                //    return Result.Fail("Please wait a few minutes before you try again.", PinLoginResult.LimitError);
                //}
                //if (loginFailReason.ErrorType == "inactive user" || loginFailReason.ErrorType == "inactive_user")
                //{
                //    return Result.Fail($"{loginFailReason.Message}\r\nHelp url: {loginFailReason.HelpUrl}", PinLoginResult.InactiveUser);
                //}
                //if (loginFailReason.ErrorType == "checkpoint_logged_out")
                //{
                //    if (!needsRelogin)
                //    {
                //        needsRelogin = true;
                //        goto ReloginLabel;
                //    }
                //    return Result.Fail($"{loginFailReason.ErrorType} {loginFailReason.CheckpointUrl}", PinLoginResult.CheckpointLoggedOut);
                //}
                //       return Result.UnExpectedResponse<PinLoginResult>(response, json);
                //}

                //var loginInfo = JsonConvert.DeserializeObject<PinLoginResponse>(json);
                //_user.UserName = loginInfo.User?.UserName;
                //IsUserAuthenticated = loginInfo.User != null;
                //if (loginInfo.User != null)
                //    _httpRequestProcessor.RequestMessage.Username = loginInfo.User.UserName;
                //var converter = ConvertersFabric.Instance.GetUserShortConverter(loginInfo.User);
                //_user.LoggedInUser = converter.Convert();
                //_user.RankToken = $"{_user.LoggedInUser.Pk}_{_httpRequestProcessor.RequestMessage.PhoneId}";
                //if (string.IsNullOrEmpty(_user.CsrfToken))
                //{
                //    cookies =
                //      _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client
                //          .BaseAddress);
                //    _user.CsrfToken = cookies[PinApiConstants.CSRFTOKEN]?.Value ?? string.Empty;
                //}
                return Result.Success(PinLoginResult.Success);
            }
            catch (HttpRequestException httpException)
            {
                _logger?.LogException(httpException);
                return Result.Fail(httpException, PinLoginResult.Exception, ResponseType.NetworkProblem);
            }
            catch (Exception exception)
            {
  //              LogException(exception);
                return Result.Fail(exception, PinLoginResult.Exception);
            }
            finally
            {
           //      
            }
        }












        #region State data

        /// <summary>
        ///     Get current state info as Memory stream
        /// </summary>
        /// <returns>
        ///     State data
        /// </returns>
        public Stream GetStateDataAsStream()
        {

            var Cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(new Uri(PinApiConstants.URL_BASE));
            var RawCookiesList = new List<Cookie>();
            foreach (Cookie cookie in Cookies)
            {
                RawCookiesList.Add(cookie);
            }


            var state = new StateData
            {
              
                IsAuthenticated = IsUserAuthenticated,
                UserSession = _user,
                Cookies = _httpRequestProcessor.HttpHandler.CookieContainer,
                RawCookies = RawCookiesList,
              
            };
            return SerializationHelper.SerializeToStream(state);
        }
        /// <summary>
        ///     Get current state info as Json string
        /// </summary>
        /// <returns>
        ///     State data
        /// </returns>
        public string GetStateDataAsString()
        {

            var Cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(new Uri(PinApiConstants.URL_BASE));
            var RawCookiesList = new List<Cookie>();
            foreach (Cookie cookie in Cookies)
            {
                RawCookiesList.Add(cookie);
            }

            var state = new StateData
            {
            
                IsAuthenticated = IsUserAuthenticated,
                UserSession = _user,
                Cookies = _httpRequestProcessor.HttpHandler.CookieContainer,
                RawCookies = RawCookiesList,
            
            };
            return SerializationHelper.SerializeToString(state);
        }

        /// <summary>
        ///     Get current state as StateData object
        /// </summary>
        /// <returns>
        ///     State data object
        /// </returns>
        public StateData GetStateDataAsObject()
        {
            var Cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(new Uri(PinApiConstants.URL_BASE));
            var RawCookiesList = new List<Cookie>();
            foreach (Cookie cookie in Cookies)
            {
                RawCookiesList.Add(cookie);
            }

            var state = new StateData
            {
               
                IsAuthenticated = IsUserAuthenticated,
                UserSession = _user,
                Cookies = _httpRequestProcessor.HttpHandler.CookieContainer,
                RawCookies = RawCookiesList,
               
            };
            return state;
        }

        /// <summary>
        ///     Get current state info as Memory stream asynchronously
        /// </summary>
        /// <returns>
        ///     State data
        /// </returns>
        public async Task<Stream> GetStateDataAsStreamAsync()
        {
            return await Task<Stream>.Factory.StartNew(() =>
            {
                var state = GetStateDataAsStream();
                Task.Delay(1000);
                return state;
            });
        }
        /// <summary>
        ///     Get current state info as Json string asynchronously
        /// </summary>
        /// <returns>
        ///     State data
        /// </returns>
        public async Task<string> GetStateDataAsStringAsync()
        {
            return await Task<string>.Factory.StartNew(() =>
            {
                var state = GetStateDataAsString();
                Task.Delay(1000);
                return state;
            });
        }
        /// <summary>
        ///     Loads the state data from stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void LoadStateDataFromStream(Stream stream)
        {
            var data = SerializationHelper.DeserializeFromStream<StateData>(stream);
           
            _user = data.UserSession;

            _httpRequestProcessor.RequestMessage.username_or_email = data.UserSession.UserName;
            _httpRequestProcessor.RequestMessage.password = data.UserSession.Password;



            foreach (var cookie in data.RawCookies)
            {
                _httpRequestProcessor.HttpHandler.CookieContainer.Add(new Uri(PinApiConstants.URL_BASE), cookie);
            }

          

            IsUserAuthenticated = data.IsAuthenticated;
            InvalidateProcessors();
        }
        /// <summary>
        ///     Set state data from provided json string
        /// </summary>
        public void LoadStateDataFromString(string json)
        {
            var data = SerializationHelper.DeserializeFromString<StateData>(json);
           
            _user = data.UserSession;

            //Load Stream Edit 
            _httpRequestProcessor.RequestMessage.username_or_email = data.UserSession.UserName;
            _httpRequestProcessor.RequestMessage.password = data.UserSession.Password;

            

            foreach (var cookie in data.RawCookies)
            {
                _httpRequestProcessor.HttpHandler.CookieContainer.Add(new Uri(PinApiConstants.URL_BASE), cookie);
            }

            IsUserAuthenticated = data.IsAuthenticated;
            InvalidateProcessors();
        }


        /// <summary>
        ///     Set state data from StateData object
        /// </summary>
        /// <param name="stateData"></param>
        public void LoadStateDataFromObject(StateData stateData)
        {
            
            _user = stateData.UserSession;

            //Load Stream Edit 
            _httpRequestProcessor.RequestMessage.username_or_email = stateData.UserSession.UserName;
            _httpRequestProcessor.RequestMessage.password = stateData.UserSession.Password;

            foreach (var cookie in stateData.RawCookies)
            {
                _httpRequestProcessor.HttpHandler.CookieContainer.Add(new Uri(PinApiConstants.URL_BASE), cookie);
            }

             
            

            IsUserAuthenticated = stateData.IsAuthenticated;
            InvalidateProcessors();
        }

        /// <summary>
        ///     Set state data from provided stream asynchronously
        /// </summary>
        public async Task LoadStateDataFromStreamAsync(Stream stream)
        {
            await Task.Factory.StartNew(() =>
            {
                LoadStateDataFromStream(stream);
                Task.Delay(1000);
            });
        }
        /// <summary>
        ///     Set state data from provided json string asynchronously
        /// </summary>
        public async Task LoadStateDataFromStringAsync(string json)
        {
            await Task.Factory.StartNew(() =>
            {
                LoadStateDataFromString(json);
                Task.Delay(1000);
            });
        }

        #endregion State data

        #region private part

        private void InvalidateProcessors()
        {

            _pinProcessor = new PinProcessor(_user, _httpRequestProcessor, _logger, _userAuthValidate, this, _httpHelper);
            _userProcessor = new UserProcessor(_user, _httpRequestProcessor, _logger, _userAuthValidate, this, _httpHelper);

        }

        //private void ValidateUserAsync(InstaUserShortResponse user, string csrfToken, bool validateExtra = true, string password = null)
        //{
        //    try
        //    {
        //   //     var converter = ConvertersFabric.Instance.GetUserShortConverter(user);
        //   //     _user.LoggedInUser = converter.Convert();
        //        if (password != null)
        //            _user.Password = password;
        //        _user.UserName = _user.UserName;
        //        if (validateExtra)
        //        {
                    
        //            _user.CsrfToken = csrfToken;
        //            if (string.IsNullOrEmpty(_user.CsrfToken))
        //            {
        //                var cookies =
        //                  _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client
        //                      .BaseAddress);
        //                _user.CsrfToken = cookies[PinApiConstants.CSRFTOKEN]?.Value ?? string.Empty;
        //            }
        //            IsUserAuthenticated = true;
        //            InvalidateProcessors();
        //        }

        //    }
        //    catch { }
        //}

        private void ValidateUser()
        {
            if (string.IsNullOrEmpty(_user.UserName) || string.IsNullOrEmpty(_user.Password))
                throw new ArgumentException("user name and password must be specified");
        }

        private void ValidateLoggedIn()
        {
            if (!IsUserAuthenticated)
                throw new ArgumentException("user must be authenticated");
        }

        private void ValidateRequestMessage()
        {
            if (_httpRequestProcessor.RequestMessage == null )
                throw new ArgumentException("API request message null or empty");
        }

        private void LogException(Exception exception)
        {
            _logger?.LogException(exception);
        }

        #endregion
    }
}
