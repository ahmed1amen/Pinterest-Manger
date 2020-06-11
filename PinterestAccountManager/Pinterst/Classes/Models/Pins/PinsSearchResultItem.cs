using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.Classes.Models
{
    public class Board
    {
        [JsonProperty("url")]
        public string url  { get; set; }
        [JsonProperty("name")]
        public string name { get; set; }

    }


    public class Orig
    {
        [JsonProperty("url")]
        public string url { get; set; }

    }


    public class Images
    {
        [JsonProperty("orig")]
        public Orig orig { get; set; }
     
    }



    public class PinsSearchResultMain

    {
        public List<PinsSearchResultItem> PinsSearchResultItem { get; set; }
        public string Cursor { get; set; }


    }


    public class PinsSearchResultItem
    {




        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("grid_title")]
        public string title { get; set; }

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("link")]
        public string link { get; set; }

        [JsonProperty("description")]
        public string description { get; set; }

        [JsonProperty("images")]
        public Images images { get; set; }



        [JsonProperty("board")]
        public Board board { get; set; }

        

    }
}