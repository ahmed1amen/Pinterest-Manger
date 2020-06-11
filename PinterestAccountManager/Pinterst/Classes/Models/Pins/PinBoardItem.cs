using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinterestAccountManager.Pinterst.Classes.Models.Pins
{



public class Owner
{

[JsonProperty("id")] public long id { get; set; }
[JsonProperty("username")] public string username { get; set; }
[JsonProperty("full_name")]public string full_name { get; set; }
[JsonProperty("image_small_url")]public string image_small_url { get; set; }

}

public class BoardImage
{
[JsonProperty("url")] public string url { get; set; }
[JsonProperty("width")] public int width { get; set; }
[JsonProperty("height")] public int height { get; set; }
}
 public class BoardItem
{


[JsonProperty("sectionless_pin_count")] public int sectionless_pin_count { get; set; }
[JsonProperty("url")] public string url { get; set; }
[JsonProperty("locale")] public string locale { get; set; }
[JsonProperty("owner")] public Owner owner { get; set; }
[JsonProperty("name")] public string name { get; set; }
[JsonProperty("map_id")] public string map_id { get; set; }
[JsonProperty("id")] public long id { get; set; }
[JsonProperty("section_count")] public int section_count { get; set; }
[JsonProperty("follower_count")] public int follower_count { get; set; }

[JsonProperty("images")]  public JObject imageFolder { get; set; }

public List<BoardImage> Images { get { return JsonConvert.DeserializeObject<List<BoardImage>>(imageFolder[imageFolder.First.Path].ToString()); } }


    }
}
