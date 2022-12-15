using System;
using System.Collections.Generic;
using System.Threading;
using CardUITest.Cards;
using CardUITest.Cards.DefaultImplementation;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardUITest.AssetLoading
{
    public class ArtLoader : MonoBehaviour
    {
        [SerializeField] private int requestedWidth = 64;
        [SerializeField] private int requestedHeight = 64;
        [SerializeField] private float pixelsPerUnit = 64f;

        private readonly HashSet<Sprite> _trackedSprites = new();
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;

        private void Awake()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        // After we are done with sprites, they will be disposed together with this object
        private void OnDestroy()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            foreach (var trackedSprite in _trackedSprites)
            {
                Destroy(trackedSprite.texture);
                Destroy(trackedSprite);
            }
        }

        /// <summary>
        /// Load CardAssets which textures and sprites are tracked and disposed with this object
        /// </summary>
        /// <param name="numOfAssets">Number of assets to generate</param>
        /// <param name="trackAssets">If true - all textures and sprites will be destroyed when this object is destroyed</param>
        /// <param name="seed">If not null - textures will be loaded using static random with this seed</param>
        /// <param name="progress">Conditional attribute to provide progress of loading. Range is normalized from 0 to 1.</param>
        /// <returns>CardAssets with generated sprites, title, description and randomized data</returns>
        public async UniTask<CardAsset[]> LoadAssets(int numOfAssets, IProgress<float> progress = null)
        {
            var imgDataLoader = new ImageDataLoader(requestedWidth, requestedHeight);
            // Load images and their info
            var imageData = await imgDataLoader.LoadRandomImageData(numOfAssets, progress, _cancellationToken);

            if (imageData is null)
            {
                return null;
            }

            // Create card assets from those
            var cards = CardGenerationUtility.GenerateCards(imageData, pixelsPerUnit);

            // Add resulting sprites into collection so that we can destroy them and avoid leaking
            foreach (var cardAsset in cards)
            {
                _trackedSprites.Add(cardAsset.portrait);
            }

            return cards;
        }
    }
}