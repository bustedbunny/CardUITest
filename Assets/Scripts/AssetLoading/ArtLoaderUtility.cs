using System;

namespace CardUITest.AssetLoading
{
    public class ArtLoaderUtility
    {
        public static int GetIDFromUrl(string url)
        {
            const string startingPart = "/id/";

            var indexOfStart = url.IndexOf(startingPart, StringComparison.CurrentCulture) + startingPart.Length;
            var sub = url[indexOfStart..];
            var indexOfEnding = sub.IndexOf('/');

            return int.Parse(sub[..indexOfEnding]);
        }
    }
}