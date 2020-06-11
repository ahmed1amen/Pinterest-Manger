using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinterestAccountManager.Pinterst.Classes;
using PinterestAccountManager.Pinterst.Classes.Models;
using PinterestAccountManager.Pinterst.Classes.Models.Pins;
using PinterestAccountManager.Pinterst.Helpers;
using PinterestAccountManager.Pinterst.Logger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PinterestAccountManager.Pinterst.API.Processors
{
  internal  class UserProcessor : IUserProcessor
    { 

        #region Properties and constructor
        private readonly HttpHelper _httpHelper;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly PinApi _PinApi;
        private readonly IPinLogger _logger;
        private readonly UserSessionData _user;
        private readonly UserAuthValidate _userAuthValidate;
        public UserProcessor(UserSessionData user,
            IHttpRequestProcessor httpRequestProcessor, IPinLogger logger,
            UserAuthValidate userAuthValidate, PinApi pinapi, HttpHelper httpHelper)
        {
            _user = user;
            _httpRequestProcessor = httpRequestProcessor;
            _logger = logger;
            _userAuthValidate = userAuthValidate;
            _httpHelper = httpHelper;
        }



        #endregion Properties and constructor



        public async Task<IResult<PinUser>> GetCurrentUserAsync()
        {

            var request = _httpHelper.GetDefaultRequest(HttpMethod.Get, UriCreator.GetUserSettingUri());
            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);
            var response = await _httpRequestProcessor.SendAsync(request);
            var json = JObject.Parse(  await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"].ToString();

            PinUser x = JsonConvert.DeserializeObject<PinUser>(popupJson);


            if (x.is_write_banned ==true)
            {
            
                return Result.Fail<PinUser>(new Exception("User Is Banned"));


            }
            return Result.Success(JsonConvert.DeserializeObject<PinUser>(popupJson));





        }

        public async Task<IResult<List<BoardItem>>>GetUserBoards(string Username ="")
        {

            if (Username == "")
                Username = _user.UserName;

            var builder = new UriBuilder("https://www.pinterest.com/" + PinApiConstants.RESOURCE_GET_BOARDS);
            builder.Port = -1;


            var query = HttpUtility.ParseQueryString(builder.Query);
            query["source_url"] = "";
            query["data"] ="{\"options\":{\"username\":\"" + Username + "\",\"field_set_key\":\"detailed\"},\"context\":{}}";
            builder.Query = query.ToString();
            string url = builder.ToString();


            var request = _httpHelper.GetDefaultRequest(HttpMethod.Get,new Uri(url));
            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);


            var response = await _httpRequestProcessor.GetAsync(new Uri(url));
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"].ToString();

            return Result.Success(JsonConvert.DeserializeObject<List<BoardItem>>(popupJson));
          }




        public async Task<IResult<PinsSearchResultMain>> SearchPins(string Query,string Cursor)
        {





            var builder = new UriBuilder("https://www.pinterest.com/" + PinApiConstants.RESOURCE_SEARCH);
            builder.Port = -1;


            var query = HttpUtility.ParseQueryString(builder.Query);
            query["source_url"] = "";
            query["data"] = query["data"] = "{\"options\":{\"scope\":\"pins\",\"query\":\"" + Query + "\"},\"context\":{}}";
             builder.Query = query.ToString();
            string url = builder.ToString();


            var request = _httpHelper.GetDefaultRequest(HttpMethod.Get, new Uri(url));


            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);


            var response = await _httpRequestProcessor.SendAsync(request);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"]["results"].ToString();
            var cursor = json["resource_response"]["bookmark"].ToString();




            List<PinsSearchResultItem> LIST = JsonConvert.DeserializeObject<List<PinsSearchResultItem>>(popupJson);


            PinsSearchResultMain pinsSearchResultMain = new PinsSearchResultMain();

            pinsSearchResultMain.PinsSearchResultItem = LIST;
            pinsSearchResultMain.Cursor = cursor;

            return Result.Success(pinsSearchResultMain);
                



        }
        public async Task<IResult<PinsSearchResultMain>> SearchPins(string Query )
        {


            var builder = new UriBuilder("https://www.pinterest.com/" + PinApiConstants.RESOURCE_SEARCH);
            builder.Port = -1;


            var query = HttpUtility.ParseQueryString(builder.Query);
            query["source_url"] = "";
            query["data"] = "{\"options\":{\"scope\":\"pins\",\"query\":\"" + Query + "\"},\"context\":{}}";
            builder.Query = query.ToString();
            string url = builder.ToString();


            var request = _httpHelper.GetDefaultRequest(HttpMethod.Get, new Uri(url));


            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);


            var response = await _httpRequestProcessor.SendAsync(request);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"]["results"].ToString();
            var cursor = json["resource_response"]["bookmark"].ToString();




            List<PinsSearchResultItem> LIST = JsonConvert.DeserializeObject<List<PinsSearchResultItem>>(popupJson);


            PinsSearchResultMain pinsSearchResultMain = new PinsSearchResultMain();

            pinsSearchResultMain.PinsSearchResultItem = LIST;
            pinsSearchResultMain.Cursor = cursor;

            return Result.Success(pinsSearchResultMain);





        }


        public async Task<IResult<PinsSearchResultMain>>GetBoardPins(string BoardID)
        {

         var fields = new Dictionary<string, string>
        {
        {"source_url",""},


        {"data","{\"options\":{\"board_id\":\"" + BoardID+ "\"},\"context\":{}}"}

        };


            var request = _httpHelper.GetDefaultRequest(HttpMethod.Get, UriCreator.GetBoardPinsUri(), fields);
            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);


            var response = await _httpRequestProcessor.SendAsync(request);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"].ToString();



            List<PinsSearchResultItem> LIST = JsonConvert.DeserializeObject<List<PinsSearchResultItem>>(popupJson);


            PinsSearchResultMain pinsSearchResultMain = new PinsSearchResultMain();

            pinsSearchResultMain.PinsSearchResultItem = LIST;
      

            return Result.Success(pinsSearchResultMain);




        }

        public async Task<IResult<PinsSearchResultMain>>GetBoardPins(string BoardID , string Cursor )
        {



            var fields = new Dictionary<string, string>
        {
        {"source_url",""},


        {"data","{\"options\":{\"board_id\":\""  + BoardID+ "\",\"bookmarks\":[\"" +Cursor + "\"]},\"context\":{}}"}

        };




            var request = _httpHelper.GetDefaultRequest(HttpMethod.Get, UriCreator.GetBoardPinsUri(), fields);
            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);


            var response = await _httpRequestProcessor.SendAsync(request);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"].ToString();
            var cursor = json["resource_response"]["bookmark"].ToString();




            List<PinsSearchResultItem> LIST = JsonConvert.DeserializeObject<List<PinsSearchResultItem>>(popupJson);


            PinsSearchResultMain pinsSearchResultMain = new PinsSearchResultMain();

            pinsSearchResultMain.PinsSearchResultItem = LIST;
            pinsSearchResultMain.Cursor = cursor;

            return Result.Success(pinsSearchResultMain);


        }

        public async Task<IResult<Owner>> GetBoardInfo(string BoardUri)
        {

            string[] boardurlArr = BoardUri.Split('/');

            var fields = new Dictionary<string, string>
        {
        {"source_url",""},
        {"data","{\"options\":{\"slug\":\""   +  boardurlArr[1] + "\",\"username\":\""  + boardurlArr[0]  + "\",\"field_set_key\":\"detailed\"},\"context\":{}}"}

        };




            var request = _httpHelper.GetDefaultRequest(HttpMethod.Get, UriCreator.GetBoardInfoUri(), fields);
            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);


            var response = await _httpRequestProcessor.SendAsync(request);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"].ToString();





            Owner BoardINFO = new Owner();


            BoardINFO =  JsonConvert.DeserializeObject<Owner>(popupJson);

         
            return Result.Success(BoardINFO);




        }
    }
}
