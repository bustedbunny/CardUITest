using System.Collections.Generic;
using CardUITest.Cards;
using CardUITest.Cards.DefaultImplementation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CardUITest.AssetLoading
{
    public static class CardGenerationUtility
    {
        // Create ready CardAsset from temporary data
        public static CardAsset[] GenerateCards(ImageData[] data, float pixelsPerUnit)
        {
            var assets = new List<CardAsset>(data.Length);

            for (int i = 0; i < data.Length; i++)
            {
                var imgData = data[i];
                if (imgData.ConvertedTexture is null || imgData.Author is null || imgData.URL is null)
                {
                    continue;
                }

                var asset = ScriptableObject.CreateInstance<CardAsset>();
                asset.title = imgData.Author;
                asset.description = imgData.URL;
                asset.portrait = GenerateSingleSprite(imgData.ConvertedTexture, pixelsPerUnit);
                asset.attack = Random.Range(1, 10);
                asset.health = Random.Range(1, 10);
                asset.mana = Random.Range(1, 10);

                assets.Add(asset);
            }

            return assets.ToArray();
        }

        // TODO Could be improved by using Atlas
        private static Sprite GenerateSingleSprite(Texture2D texture, float pixelsPerUnit)
        {
            var width = texture.width;
            var height = texture.height;

            var fullTextureRect = new Rect(Vector2.zero, new Vector2(width, height));
            var sprite = Sprite.Create(texture, fullTextureRect, new Vector2(0.5f, 0.5f), pixelsPerUnit);
            return sprite;
        }
    }
}