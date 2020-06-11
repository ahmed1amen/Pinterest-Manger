using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PinterestAccountManager.Pinterst.Classes;
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

namespace PinterestAccountManager.Pinterst.API.Processors
{
    internal class PinProcessor : IPinProcessor
    {
        #region Properties and constructor




        private readonly HttpHelper _httpHelper;
        private readonly IHttpRequestProcessor _httpRequestProcessor;
        private readonly PinApi _PinApi;
        private readonly IPinLogger _logger;
        private readonly UserSessionData _user;
        private readonly UserAuthValidate _userAuthValidate;
        public PinProcessor(UserSessionData user,
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



         //public async Task<IResult<bool>> GetBoards
        public async Task<IResult<PinImage>> UploadPhotoAsync(string PathOfImage)
        {

            MultipartFormDataContent form = new MultipartFormDataContent("-------------" + DateTime.Now.Ticks.ToString("x"));
            byte[] imagex = File.ReadAllBytes(PathOfImage);
            var imageContent = new ByteArrayContent(imagex);
            imageContent.Headers.Add("Content-Type", "image/png");
            imageContent.Headers.Add("Content-Disposition", "form-data; name=\"img\"; filename=" + "\"" + Path.GetFileName(PathOfImage) + "\"");
            form.Add(imageContent);

            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);

            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("DNT", "1");
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("Host", PinApiConstants.HOST);
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("Origin", PinApiConstants.URL_BASE);
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("Referer", PinApiConstants.URL_BASE);
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("User-Agent", PinApiConstants.UserAgent_Defualt);
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("X-APP-VERSION", "f9d1262");
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("X-CSRFToken", cookies[PinApiConstants.CSRFTOKEN]?.Value);
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("X-NEW-APP", "1");
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("X-Pinterest-AppState", "active");
            _httpRequestProcessor.Client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            var uploadreqresponse = await _httpRequestProcessor.Client.PostAsync(UriCreator.GetUploadUri(), form);
            ///  var uploadreqresponse = await _httpRequestProcessor.SendAsync(uploadreq);
            var uploadreqjson = await uploadreqresponse.Content.ReadAsStringAsync();

            return Result.Success(JsonConvert.DeserializeObject<PinImage>(uploadreqjson));


        }
        public async Task<IResult<bool>> PostPinAsync(string ImageUrl, string BoardID, string Description, string Link, string Title, string SectionID = "")
        {

           
            var optionvalues = new Dictionary<string, string>
                {
                { "method" , "scraped" },
                { "description" , Description},
                { "link" , Link},
                { "image_url" ,ImageUrl},
                { "board_id" , BoardID},
                { "title" , Title},

               };


            if (SectionID != "/pin-builder/")
                optionvalues.Add("section", SectionID);

            string jsonContet = JsonConvert.SerializeObject(optionvalues, Formatting.Indented);

            var fields = new Dictionary<string, string>
                {

            { "source_url", ""},
             { "data", "{\"options\":" + jsonContet + ",\"context\":{}}" }

            };
       
            



            var request = _httpHelper.GetDefaultRequest(HttpMethod.Post, UriCreator.GetPostPinUri(), fields);
            var cookies = _httpRequestProcessor.HttpHandler.CookieContainer.GetCookies(_httpRequestProcessor.Client.BaseAddress);
            _httpHelper.AssginDefaultHeaders(ref request, cookies[PinApiConstants.CSRFTOKEN]?.Value);


            var response = await _httpRequestProcessor.SendAsync(request);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            var popupJson = json["resource_response"]["data"].ToString();

            return Result.Success(true);
        }




       
    }
}
