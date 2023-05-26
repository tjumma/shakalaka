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

        public Action LocalServerButtonClicked;
        public Action LocalHostButtonClicked;
        public Action LocalClientButtonClicked;
        
        public Action RelayHostButtonClicked;
        public Action<string> RelayClientButtonClicked;

        private VisualElement _root;
        private Label _playerIdLabel;

        private Button _playLocallyButton;
        private Button _playWithRelayButton;
        private Button _quitButton;

        private VisualElement _playLocallyPopup;
        private Button _playLocallyServerButton;
        private Button _playLocallyHostButton;
        private Button _playLocallyJoinButton;

        private VisualElement _playWithRelayPopup;
        private Button _playWithRelayHostButton;
        private Button _playWithRelayJoinButton;
        private TextField _playWithRelayCodeField;

        private void Awake()
        {
            _root = document.rootVisualElement;
            _playerIdLabel = _root.Q<Label>("player-id-label");

            _playLocallyButton = _root.Q<Button>("play-locally-button");
            _playWithRelayButton = _root.Q<Button>("play-with-relay-button");
            _quitButton = _root.Q<Button>("quit-button");

            _playLocallyPopup = _root.Q<VisualElement>("play-locally-popup");
            _playLocallyServerButton = _playLocallyPopup.Q<Button>("server-button");
            _playLocallyHostButton = _playLocallyPopup.Q<Button>("host-button");
            _playLocallyJoinButton = _playLocallyPopup.Q<Button>("join-button");

            _playWithRelayPopup = _root.Q<VisualElement>("play-with-relay-popup");
            _playWithRelayHostButton = _playWithRelayPopup.Q<Button>("host-button");
            _playWithRelayJoinButton = _playWithRelayPopup.Q<Button>("join-button");
            _playWithRelayCodeField = _playWithRelayPopup.Q<TextField>("relay-code-field");
        }

        private void OnEnable()
        {
            _playLocallyButton.clicked += OnPlayLocallyButtonClicked;
            _playWithRelayButton.clicked += OnPlayWithRelayButtonClicked;
            _quitButton.clicked += OnQuitButtonClicked;

            _playLocallyServerButton.clicked += OnPlayLocallyServerButtonClicked;
            _playLocallyHostButton.clicked += OnPlayLocallyHostButtonClicked;
            _playLocallyJoinButton.clicked += OnPlayLocallyJoinButtonClicked;
            
            _playWithRelayHostButton.clicked += OnPlayWithRelayHostButtonClicked;
            _playWithRelayJoinButton.clicked += OnPlayWithRelayJoinButtonClicked;
        }

        private void OnDisable()
        {
            _playWithRelayButton.clicked -= OnPlayWithRelayButtonClicked;
            _quitButton.clicked -= OnQuitButtonClicked;

            _playWithRelayHostButton.clicked -= OnPlayWithRelayHostButtonClicked;
            _playWithRelayJoinButton.clicked -= OnPlayWithRelayJoinButtonClicked;
        }
        
        private void OnPlayLocallyButtonClicked()
        {
            Debug.Log("PlayLocally clicked");
            _playLocallyPopup.style.visibility = Visibility.Visible;
        }

        private void OnPlayWithRelayButtonClicked()
        {
            Debug.Log("PlayWithRelay clicked");
            _playWithRelayPopup.style.visibility = Visibility.Visible;
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
        
        private void OnPlayLocallyServerButtonClicked()
        {
            LocalServerButtonClicked?.Invoke();
        }
        
        private void OnPlayLocallyHostButtonClicked()
        {
            LocalHostButtonClicked?.Invoke();
        }
        
        private void OnPlayLocallyJoinButtonClicked()
        {
            LocalClientButtonClicked?.Invoke();
        }

        private void OnPlayWithRelayHostButtonClicked()
        {
            RelayHostButtonClicked?.Invoke();
        }

        private void OnPlayWithRelayJoinButtonClicked()
        {
            RelayClientButtonClicked?.Invoke(_playWithRelayCodeField.text);
        }

        public void SetPlayerId(string playerId)
        {
            _playerIdLabel.text = $"PlayerId: {playerId}";
        }
    }
}