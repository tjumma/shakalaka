using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class ClientBoardMVP : MonoBehaviour
    {
        [SerializeField] private CardsPile playerHand;
        [SerializeField] private CardsPile opponentHand;

        [SerializeField] private GameObject cardPrefab;

        private IObjectResolver _container;

        [Inject]
        public void Construct(IObjectResolver container)
        {
            _container = container;
        }

        public void SetupBoard(ClientBoard clientBoard)
        {
            SetupPile(playerHand, clientBoard.playerPile);
            SetupPile(opponentHand, clientBoard.opponentPile);
        }

        private void SetupPile(CardsPile pileView, Pile pile)
        {
            pileView.faceDown = pile.visibility == PileVisibility.InvisibleForPlayer;
            
            foreach (var cardType in pile.cards)
            {
                var cardObject = _container.Instantiate(cardPrefab);
                cardObject.GetComponent<CardView>().SetType(cardType);
                pileView.Add(cardObject, false);
            }
        }
    }
}