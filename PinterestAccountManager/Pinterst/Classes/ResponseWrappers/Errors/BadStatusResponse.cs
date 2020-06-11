
using Newtonsoft.Json;
using PinterestAccountManager.Pinterst.Classes.ResponseWrappers.BaseResponse;

namespace  PinterestAccountManager.Pinterst.Classes.ResponseWrappers
{
    public class BadStatusResponse : BaseStatusResponse
    {
        [JsonProperty("message")] public string Message { get; set; }

        [JsonProperty("error_type")] public string ErrorType { get; set; }

        [JsonProperty("checkpoint_url")] public string CheckPointUrl { get; set; }

        [JsonProperty("spam")] public bool Spam { get; set; }

        [JsonProperty("feedback_title")] public string FeedbackTitle { get; set; }

        [JsonProperty("feedback_message")] public string FeedbackMessage { get; set; }

  
    }
}