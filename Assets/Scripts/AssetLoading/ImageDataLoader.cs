using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace CardUITest.AssetLoading
{
    /// <summary>
    /// This struct is used to load random images from https://picsum.photos/
    /// </summary>
    public readonly struct ImageDataLoader
    {
        public ImageDataLoader(int requestedWidth, int requestedHeight)
        {
            const string url = "https://picsum.photos/";
            var sb = new StringBuilder();
            sb.Append(url);
            sb.Append($"{requestedWidth}/{requestedHeight}/");
            _url = sb.ToString();
        }

        private readonly string _url;

        public async UniTask<ImageData[]> LoadRandomImageData(int numToLoad, IProgress<float> progress = null,
            CancellationToken ct = default)
        {
            var result = new ImageData[numToLoad];


            for (int i = 0; i < numToLoad; i++)
            {
                // If cancellation requested, dispose all allocated textures
                if (ct.IsCancellationRequested)
                {
                    for (int j = 0; j < i; j++)
                    {
                        var curData = result[i];
                        if (curData.ConvertedTexture is null) continue;
                        Object.DestroyImmediate(result[j].ConvertedTexture);
                    }

                    return null;
                }

                // First we load image
                var loadedTexture = await AssetLoadingUtility.LoadTextureAsync(_url, ct);

                if (ct.IsCancellationRequested) continue;

                // If everything is fine we obtain id from resulting img url
                var id = ArtLoaderUtility.GetIDFromUrl(loadedTexture.URL);
                // Then we make a url to load info data about this img
                // This part is not important, but I found it a nice way to get some placeholder texts for cards
                var detailsUrl = $"https://picsum.photos/id/{id}/info";

                var data = await AssetLoadingUtility.LoadJsonAsync<ImageData>(detailsUrl, ct);

                if (ct.IsCancellationRequested) continue;

                data.ConvertedTexture = loadedTexture.Texture;
                result[i] = data;

                // Once everything is done we update our progress
                progress?.Report((float)i / numToLoad);
            }

            return result;
        }
    }
}