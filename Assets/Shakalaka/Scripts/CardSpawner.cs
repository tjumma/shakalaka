using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class CardSpawner : NetworkBehaviour
    {
        [SerializeField] private CardsPile playerHand;
        [SerializeField] private CardsPile enemyHand;

        [SerializeField] private GameObject cardPrefab;

        private IObjectResolver _container;

        [Inject]
        public void Construct(IObjectResolver container)
        {
            _container = container;
        }

        public void FillPlayerHand(int[] playerCards)
        {
            foreach (var cardType in playerCards)
            {
                var cardObject = _container.Instantiate(cardPrefab);
                cardObject.GetComponent<Card>().SetType(cardType);
                playerHand.Add(cardObject, false);
            }
        }
    }
}