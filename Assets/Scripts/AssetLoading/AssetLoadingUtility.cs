using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace CardUITest.AssetLoading
{
    public static class AssetLoadingUtility
    {
        public static async UniTask<T> LoadJsonAsync<T>(string url, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return default;

            var bufferHandler = new DownloadHandlerBuffer();
            using var webRequest = UnityWebRequest.Get(url);
            webRequest.downloadHandler = bufferHandler;

#pragma warning disable CS4014
            webRequest.SendWebRequest();
#pragma warning restore CS4014

            // ReSharper disable once AccessToDisposedClosure
            await UniTask.WaitUntil(() => webRequest.isDone, cancellationToken: ct).SuppressCancellationThrow();

            if (ct.IsCancellationRequested)
            {
                webRequest.Abort();
                return default;
            }

            return JsonConvert.DeserializeObject<T>(bufferHandler.text);
        }

        public static async UniTask<LoadedTexture> LoadTextureAsync(string url, CancellationToken ct)
        {
            if (ct.IsCancellationRequested) return default;

            var textureHandler = new DownloadHandlerTexture();
            using var imgRequest = UnityWebRequest.Get(url);
            imgRequest.downloadHandler = textureHandler;

#pragma warning disable CS4014
            imgRequest.SendWebRequest();
#pragma warning restore CS4014

            // ReSharper disable once AccessToDisposedClosure
            await UniTask.WaitUntil(() => imgRequest.isDone, cancellationToken: ct).SuppressCancellationThrow();

            if (ct.IsCancellationRequested)
            {
                imgRequest.Abort();
                return default;
            }

            return new LoadedTexture(imgRequest.url, textureHandler.texture);
        }
    }
}