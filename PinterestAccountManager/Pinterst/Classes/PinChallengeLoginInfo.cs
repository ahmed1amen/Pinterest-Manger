using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.Classes
{
   public class PinChallengeLoginInfo
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("api_path")]
        public string ApiPath { get; set; }
        [JsonProperty("hide_webview_header")]
        public bool HideWebviewHeader { get; set; }
        [JsonProperty("lock")]
        public bool Lock { get; set; }
        [JsonProperty("logout")]
        public bool Logout { get; set; }
        [JsonProperty("native_flow")]
        public bool NativeFlow { get; set; }

    }
}
