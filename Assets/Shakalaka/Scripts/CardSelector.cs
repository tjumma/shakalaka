using UnityEngine;

namespace Shakalaka
{
    public class CardSelector : MonoBehaviour
    {
        [SerializeField] private Transform selectedCardParent;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private LayerMask cardMask;
        [SerializeField] private LayerMask playingAreaMask;

        private Plane _selectedCardPilePlane;
        private GameObject _selectedCard;
        private CardsPile _selectedCardOriginPile;
        private int? _selectedCardPreviousIndex;

        private bool _isCardSelected;

        private void OnEnable()
        {
            inputManager.OnStartTouch += TrySelectCard;
            inputManager.OnEndTouch += TryReleaseCard;
        }
        private void OnDisable()
        {
            inputManager.OnStartTouch -= TrySelectCard;
            inputManager.OnEndTouch -= TryReleaseCard;
        }

        private void Start()
        {
            _selectedCardPilePlane = new Plane(Vector3.down, 1f);
        }

        private void Update()
        {
            if (!_isCardSelected)
                return;
            
            Ray ray = Camera.main.ScreenPointToRay(inputManager.TouchPosition);

            if (_selectedCardPilePlane.Raycast(ray, out var distance))
            {
                selectedCardParent.transform.position = ray.GetPoint(distance);
            }
        }
        
        private void TrySelectCard(Vector2 touchPosition, float time)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, cardMask))
            {
                _isCardSelected = true;
                // Destroy(hit.collider.gameObject);
                _selectedCard = hit.collider.gameObject;
                _selectedCardOriginPile = hit.collider.gameObject.GetComponentInParent<CardsPile>();
                _selectedCardPreviousIndex = _selectedCardOriginPile.Remove(_selectedCard);
                
                _selectedCard.transform.SetParent(selectedCardParent, false);
                _selectedCard.transform.localPosition = Vector3.zero;
                _selectedCard.transform.localRotation = Quaternion.Euler(90, 0,0);
            }
        }
        
        private void TryReleaseCard(Vector2 touchPosition, float time)
        {
            if (_selectedCard == null)
                return;
            
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, playingAreaMask))
            {
                var newCardsPile = hit.transform.gameObject.GetComponentInParent<CardsPile>();
                
                newCardsPile.Add(_selectedCard);
                
                // var cardPos = _selectedCard.transform.position;
                // cardPos.y = 0f;
                // _selectedCard.transform.SetParent(null);
                // _selectedCard.transform.position = cardPos;
            }
            else
            {
                if (_selectedCardPreviousIndex != null)
                    _selectedCardOriginPile.Add(_selectedCard, _selectedCardPreviousIndex.Value);
            }
            
            _selectedCard = null;
            _selectedCardOriginPile = null;
            _selectedCardPreviousIndex = null;
            _isCardSelected = false;
        }
    }
}