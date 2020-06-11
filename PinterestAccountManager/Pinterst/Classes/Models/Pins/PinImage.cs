using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.Classes.Models.Pins
{
  public  class PinImage
    {
            
        [JsonProperty("image_url")]
        public string image_url { get; set; }
        [JsonProperty("width")]
        public int width { get; set; }
        [JsonProperty("height")]
        public string height { get; set; }
        [JsonProperty("filename")]
        internal string filename { get; set; }
        [JsonProperty("success")]
        internal string success { get; set; }
    }
}
