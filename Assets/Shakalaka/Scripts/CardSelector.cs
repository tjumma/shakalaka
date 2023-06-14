using System;
using UnityEngine;

namespace Shakalaka
{
    public class CardSelector : MonoBehaviour
    {
        [SerializeField] private InputManager inputManager;

        private void OnEnable()
        {
            inputManager.OnFingerDown += TrySelectCard;
        }
        private void OnDisable()
        {
            inputManager.OnFingerDown -= TrySelectCard;
        }
        
        private void TrySelectCard(Vector2 touchPosition, float time)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);

            if (Physics.Raycast(ray, out var hit))
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
}