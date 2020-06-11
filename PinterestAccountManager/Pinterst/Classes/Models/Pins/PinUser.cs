using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.Classes.Models
{
  public  class PinUser
    {


        [JsonProperty("id")]
        public long  id { get; set; }



        [JsonProperty("email")]
        public string email { get; set; }


        [JsonProperty("locale")]
        public string locale { get; set; }

        [JsonProperty("gender")]
        public string gender { get; set; }

        [JsonProperty("first_name")]
        public string first_name { get; set; }

        [JsonProperty("last_name")]
        public string last_name { get; set; }





        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("about")]
        public string about { get; set; }



        [JsonProperty("has_confirmed_email")]
        public bool has_confirmed_email { get; set; }


        [JsonProperty("is_write_banned")]
        public bool is_write_banned { get; set; }



        [JsonProperty("is_high_risk")]
        public bool is_high_risk { get; set; }


        [JsonProperty("profile_image_url")]
        public string profile_image_url { get; set; }



    }
}
