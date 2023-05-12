using System;
namespace tinyurl_2.Controllers
{
    public class KeyValue
    {
        public string url { get; set; }

        [Newtonsoft.Json.JsonProperty(PropertyName = "id")]
        public string code { get; set; }
    }

}

