using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shakalaka
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private UIDocument document;

        public Action HostButtonClicked;
        public Action<string> JoinButtonClicked;
        
        
        private VisualElement _root;
        private Label _playerIdLabel;

        private Button _instantPlayWithRelayButton;
        private Button _quitButton;

        private VisualElement _instantPlayWithRelayPanel;
        private Button _hostButton;
        private Button _joinButton;
        private TextField _relayCodeField;

        private void Awake()
        {
            _root = document.rootVisualElement;
            _playerIdLabel = _root.Q<Label>("player-id-label");

            _instantPlayWithRelayButton = _root.Q<Button>("instant-play-with-relay-button");
            _quitButton = _root.Q<Button>("quit-button");

            _instantPlayWithRelayPanel = _root.Q<VisualElement>("instant-play-with-relay-panel");
            _hostButton = _instantPlayWithRelayPanel.Q<Button>("host-button");
            _joinButton = _instantPlayWithRelayPanel.Q<Button>("join-button");
            _relayCodeField = _instantPlayWithRelayPanel.Q<TextField>("relay-code-field");
        }

        private void OnEnable()
        {
            _instantPlayWithRelayButton.clicked += OnInstantPlayWithRelayClicked;
            _quitButton.clicked += OnQuitButtonClicked;
            
            _hostButton.clicked += OnHostButtonClicked;
            _joinButton.clicked += OnJoinButtonClicked;
        }

        private void OnDisable()
        {
            _instantPlayWithRelayButton.clicked -= OnInstantPlayWithRelayClicked;
            _quitButton.clicked -= OnQuitButtonClicked;
            
            _hostButton.clicked -= OnHostButtonClicked;
            _joinButton.clicked -= OnJoinButtonClicked;
        }
        
        private void OnInstantPlayWithRelayClicked()
        {
            Debug.Log("InstantPlayWithRelay clicked");
            _instantPlayWithRelayPanel.style.visibility = Visibility.Visible;
        }
        
        private void OnQuitButtonClicked()
        {
            Debug.Log("Quit clicked");
            
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
        
        private void OnHostButtonClicked()
        {
            // _root.style.visibility = Visibility.Hidden;
            // document.enabled = false;
            // relay.CreateRelay().Forget();
            
            HostButtonClicked?.Invoke();
        }
        
        private void OnJoinButtonClicked()
        {
            // _root.style.visibility = Visibility.Hidden;
            // document.enabled = false;
            // relay.JoinRelay(_relayCodeField.text).Forget();
            
            JoinButtonClicked?.Invoke(_relayCodeField.text);
        }

        public void SetPlayerId(string playerId)
        {
            _playerIdLabel.text = $"PlayerId: {playerId}";
        }
    }
}