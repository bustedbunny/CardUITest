using UnityEngine;

namespace CardUITest.AssetLoading
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="URL">Resulting URL from redirection</param>
    /// <param name="Texture">Downloaded texture</param>
    public record LoadedTexture(string URL, Texture2D Texture);
}