
using InstagramApiSharp.Classes.ResponseWrappers;
using Newtonsoft.Json;
using PinterestAccountManager.Pinterst.Classes.ResponseWrappers.BaseResponse;

namespace PinterestAccountManager.Pinterst.Classes.ResponseWrappers
{
    public class BadStatusErrorsResponse : BaseStatusResponse
    {
        [JsonProperty("message")] public MessageErrorsResponse Message { get; set; }
    }
}