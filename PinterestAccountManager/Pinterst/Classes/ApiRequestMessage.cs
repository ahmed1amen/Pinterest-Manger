using Newtonsoft.Json;

namespace PinterestAccountManager.Pinterst.Classes
{
    internal class ApiRequestChallengeMessage : ApiRequestMessage
    {
        [JsonProperty("_csrftoken")]
        public string CsrtToken { get; set; }
    }

    public class ApiRequestMessage
    {

        [JsonProperty("username_or_email")]
        public string username_or_email { get; set; }


        [JsonProperty("password")]
        public string password { get; set; }


        internal string GetMessageString()
        {
            var json = JsonConvert.SerializeObject(this);
            return json;
        }




    }
}