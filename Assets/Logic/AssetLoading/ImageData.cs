using Newtonsoft.Json;
using UnityEngine;

namespace CardUITest.AssetLoading
{
    // Temporary data type to hold data from web requests
    public struct ImageData
    {
        [JsonProperty("author")] public string Author { get; set; }
        [JsonProperty("url")] public string URL { get; set; }
        public Texture2D ConvertedTexture;
    }
}