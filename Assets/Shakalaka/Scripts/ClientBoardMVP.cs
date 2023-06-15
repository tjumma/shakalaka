using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class ClientBoardMVP : MonoBehaviour
    {
        [SerializeField] private CardsPile playerHand;
        [SerializeField] private CardsPile opponentHand;
        [SerializeField] private CardsPile playerArea;
        [SerializeField] private CardsPile opponentArea;

        [SerializeField] private GameObject cardPrefab;

        private IObjectResolver _container;

        [Inject]
        public void Construct(IObjectResolver container)
        {
            _container = container;
        }

        public void SetupBoard(ClientBoardData clientBoardData)
        {
            SetupPile(playerHand, clientBoardData.playerHandData);
            SetupPile(opponentHand, clientBoardData.opponentHandData);
            SetupPile(playerArea, clientBoardData.playerAreaData);
            SetupPile(opponentArea, clientBoardData.opponentAreaData);
        }

        private void SetupPile(CardsPile pileView, PileData pileData)
        {
            pileView.RemoveAll(destroyCardObject: true);
            
            pileView.faceDown = pileData.visibility == PileVisibility.InvisibleForPlayer;
            
            foreach (var cardType in pileData.cards)
            {
                var cardObject = _container.Instantiate(cardPrefab);
                cardObject.GetComponent<CardView>().SetType(cardType);
                pileView.Add(cardObject, false);
            }
        }
    }
}