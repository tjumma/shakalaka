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

        public Action<string, ushort> MultiplayerJoinServerButtonClicked;

        private VisualElement _root;
        private Label _playerIdLabel;

        private Button _playLocallyButton;
        private Button _playWithRelayButton;
        private Button _multiplayerButton;
        private Button _quitButton;

        private VisualElement _playLocallyPopup;
        private Button _playLocallyServerButton;
        private Button _playLocallyHostButton;
        private Button _playLocallyJoinButton;
        private Button _closePlayLocallyPopupButton;

        private VisualElement _playWithRelayPopup;
        private Button _playWithRelayHostButton;
        private Button _playWithRelayJoinButton;
        private TextField _playWithRelayCodeField;
        private Button _closePlayWithRelayPopupButton;

        private VisualElement _multiplayerPopup;
        private TextField _multiplayerServerIpField;
        private TextField _multiplayerServerPortField;
        private Button _multiplayerJoinServerButton;
        private Button _closeMultiplayerPopupButton;

        private void Awake()
        {
            _root = document.rootVisualElement;
            _playerIdLabel = _root.Q<Label>("player-id-label");

            _playLocallyButton = _root.Q<Button>("play-locally-button");
            _playWithRelayButton = _root.Q<Button>("play-with-relay-button");
            _multiplayerButton = _root.Q<Button>("multiplayer-button");
            _quitButton = _root.Q<Button>("quit-button");

            _playLocallyPopup = _root.Q<VisualElement>("play-locally-popup");
            _playLocallyServerButton = _playLocallyPopup.Q<Button>("server-button");
            _playLocallyHostButton = _playLocallyPopup.Q<Button>("host-button");
            _playLocallyJoinButton = _playLocallyPopup.Q<Button>("join-button");
            _closePlayLocallyPopupButton = _playLocallyPopup.Q<Button>("close-popup-button");

            _playWithRelayPopup = _root.Q<VisualElement>("play-with-relay-popup");
            _playWithRelayHostButton = _playWithRelayPopup.Q<Button>("host-button");
            _playWithRelayJoinButton = _playWithRelayPopup.Q<Button>("join-button");
            _playWithRelayCodeField = _playWithRelayPopup.Q<TextField>("relay-code-field");
            _closePlayWithRelayPopupButton = _playWithRelayPopup.Q<Button>("close-popup-button");

            _multiplayerPopup = _root.Q<VisualElement>("multiplayer-popup");
            _multiplayerServerIpField = _multiplayerPopup.Q<TextField>("server-ip-field");
            _multiplayerServerPortField = _multiplayerPopup.Q<TextField>("server-port-field");
            _multiplayerJoinServerButton = _multiplayerPopup.Q<Button>("server-join-button");
            _closeMultiplayerPopupButton = _multiplayerPopup.Q<Button>("close-popup-button");
        }

        private void OnEnable()
        {
            _playLocallyButton.clicked += () => OpenPopup(_playLocallyPopup);
            _playWithRelayButton.clicked += () => OpenPopup(_playWithRelayPopup);
            _multiplayerButton.clicked += () => OpenPopup(_multiplayerPopup);
            _quitButton.clicked += OnQuitButtonClicked;

            _playLocallyServerButton.clicked += OnPlayLocallyServerButtonClicked;
            _playLocallyHostButton.clicked += OnPlayLocallyHostButtonClicked;
            _playLocallyJoinButton.clicked += OnPlayLocallyJoinButtonClicked;
            _closePlayLocallyPopupButton.clicked += () => ClosePopup(_playLocallyPopup);

            _playWithRelayHostButton.clicked += OnPlayWithRelayHostButtonClicked;
            _playWithRelayJoinButton.clicked += OnPlayWithRelayJoinButtonClicked;
            _closePlayWithRelayPopupButton.clicked += () => ClosePopup(_playWithRelayPopup);

            _multiplayerJoinServerButton.clicked += OnMultiplayerJoinServerButtonClicked;
            _closeMultiplayerPopupButton.clicked += () => ClosePopup(_multiplayerPopup);
        }

        private void OnMultiplayerJoinServerButtonClicked()
        {
            Debug.Log("JoinServer clicked");
            MultiplayerJoinServerButtonClicked?.Invoke(_multiplayerServerIpField.text,
                ushort.Parse(_multiplayerServerPortField.text));
        }

        private void ClosePopup(VisualElement popup)
        {
            popup.style.visibility = Visibility.Hidden;
        }

        private void OpenPopup(VisualElement popup)
        {
            popup.style.visibility = Visibility.Visible;
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