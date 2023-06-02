using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shakalaka
{
    [RequireComponent(typeof(UIDocument))]
    public class PreGameUI : NetworkBehaviour
    {
        [SerializeField] private UIDocument document;
        
        private VisualElement _root;
        private Label _playersConnectedLabel;

        private void Awake()
        {
            _root = document.rootVisualElement;
            _playersConnectedLabel = _root.Q<Label>("players-connected-label");
        }

        [ClientRpc]
        public void SetPlayersConnectedClientRpc(int playersCount)
        {
            _playersConnectedLabel.text = $"Players connected: {playersCount}";
        }
    }
}