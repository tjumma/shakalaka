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
            if (document == null)
                return;
            
            _root = document.rootVisualElement;
            _playersConnectedLabel = _root.Q<Label>("players-connected-label");
        }

        [ClientRpc]
        public void SetPlayersConnectedClientRpc(int playersCount)
        {
            if (_playersConnectedLabel == null)
                return;
            
            _playersConnectedLabel.text = $"Players connected: {playersCount}";
        }
    }
}