using UnityEngine;
using VContainer;

namespace Shakalaka
{
    public class CardSelector : MonoBehaviour
    {
        [SerializeField] private Transform selectedCardParent;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private LayerMask cardMask;
        [SerializeField] private LayerMask playingAreaMask;
        
        private Camera _mainCamera;

        private Plane _selectedCardPilePlane;
        private GameObject _selectedCard;
        private CardsPile _selectedCardOriginPile;
        private int? _selectedCardPreviousIndex;
        private bool _isCardSelected;

        [Inject]
        private void Construct(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }

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
            
            Ray ray = _mainCamera.ScreenPointToRay(inputManager.TouchPosition);

            if (_selectedCardPilePlane.Raycast(ray, out var distance))
            {
                selectedCardParent.transform.position = ray.GetPoint(distance);
            }
        }
        
        private void TrySelectCard(Vector2 touchPosition, float time)
        {
            if (Camera.main == null)
                return;
                
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, cardMask))
            {
                _isCardSelected = true;
                // Destroy(hit.collider.gameObject);
                _selectedCard = hit.collider.gameObject;
                _selectedCardOriginPile = _selectedCard.GetComponentInParent<CardsPile>();

                if (!_selectedCardOriginPile.isPlayerControlled)
                    return;
                
                Debug.Log($"<color=green>Player card selected!</color>");
                
                _selectedCardPreviousIndex = _selectedCardOriginPile.Remove(_selectedCard);
                _selectedCard.transform.SetParent(selectedCardParent, false);
                _selectedCard.transform.localPosition = Vector3.zero;
                _selectedCard.transform.localRotation = Quaternion.Euler(0, _selectedCardOriginPile.faceDown? 0 : 180,0);
            }
        }
        
        private void TryReleaseCard(Vector2 touchPosition, float time)
        {
            if (_selectedCard == null)
                return;
            
            Ray ray = _mainCamera.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out var hit, Mathf.Infinity, playingAreaMask))
            {
                var newCardsPile = hit.transform.gameObject.GetComponentInParent<CardsPile>();
                
                newCardsPile.Add(_selectedCard);

                GameScope.Instance.Player.RequestCardMove();

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