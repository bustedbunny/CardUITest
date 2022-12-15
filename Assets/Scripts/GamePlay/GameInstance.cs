using System;
using System.Collections.Generic;
using System.Threading;
using CardUITest.AssetLoading;
using CardUITest.Cards;
using CardUITest.Cards.DefaultImplementation;
using CardUITest.Cards.Hand;
using CardUITest.UI.Presentation;
using CardUITest.UI.Presentation.Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CardUITest.GamePlay
{
    public class GameInstance : MonoBehaviour
    {
        [SerializeField] private CardAsset defaultAsset;
        [SerializeField] private DefaultCardObject prefab;
        [SerializeField] private ArtLoader artLoader;


        [SerializeField] private LoadingScreenPagePresentation loadingScreen;
        [SerializeField] private PlayPagePresentation playPage;
        [SerializeField] private StartingPagePresentation startingPage;
        [SerializeField] private Shell shell;


        [SerializeField] private CardHand cardHand;


        [SerializeField] private bool downloadPictures;


        private CancellationTokenSource _cts;

        private void OnDestroy()
        {
            if (_cts is not null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
                _cts.Dispose();
            }
        }

        public void StartTheGame()
        {
            _cts = new CancellationTokenSource();
            if (downloadPictures)
            {
                SpawnSomeCards(_cts.Token).Forget();
            }
            else
            {
                SpawnSomeCardsDebug(_cts.Token).Forget();
            }
        }

        public void StopTheGame()
        {
            _cts.Cancel();
            _cts.Dispose();
            foreach (var card in _cards)
            {
                cardHand.RemoveCard(card);
                Destroy(card.gameObject);
            }

            _cards.Clear();

            shell.SetPage(startingPage);
        }

        private HashSet<DefaultCardObject> _cards = new();

        private async UniTaskVoid SpawnSomeCardsDebug(CancellationToken ct)
        {
            shell.SetPage(playPage);
            for (var i = 0; i < 12; i++)
            {
                var newObj = Instantiate(prefab);
                newObj.SetAsset(defaultAsset);
                newObj.transform.position = new Vector3(i * 2, 0f, 0f);

                _cards.Add(newObj);
                cardHand.AddCard(newObj.GetComponent<IHeldableCard>());

                await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: ct);
            }
        }

        private async UniTaskVoid SpawnSomeCards(CancellationToken ct)
        {
            var cardsAssets = await artLoader.LoadAssets(15, loadingScreen.StartLoadingScreenWithProgressBar());

            if (cardsAssets is null)
            {
                return;
            }

            shell.SetPage(playPage);
            for (var i = 0; i < cardsAssets.Length; i++)
            {
                var asset = cardsAssets[i];
                var newObj = Instantiate(prefab);
                newObj.SetAsset(asset);
                newObj.transform.position = new Vector3(i * 2, 0f, 0f);

                _cards.Add(newObj);
                cardHand.AddCard(newObj.GetComponent<IHeldableCard>());

                await UniTask.Delay(TimeSpan.FromSeconds(0.5), cancellationToken: ct);
            }
        }
    }
}