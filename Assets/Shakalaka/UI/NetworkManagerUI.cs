using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Shakalaka.UI
{
    public class NetworkManagerUI : MonoBehaviour
    {
        [SerializeField] private UIDocument document;

        private VisualElement _root;

        private Button _serverButton;
        private Button _clientButton;
        private Button _hostButton;

        private void Awake()
        {
            _root = document.rootVisualElement;
            
            _serverButton = _root.Q<Button>("server-button");
            _clientButton = _root.Q<Button>("client-button");
            _hostButton = _root.Q<Button>("host-button");
        }

        private void OnEnable()
        {
            _serverButton.clicked += OnServerButtonClicked;
            _clientButton.clicked += OnClientButtonClicked;
            _hostButton.clicked += OnHostButtonClicked;
        }

        private void OnDisable()
        {
            _serverButton.clicked -= OnServerButtonClicked;
            _clientButton.clicked -= OnClientButtonClicked;
            _hostButton.clicked -= OnHostButtonClicked;
        }

        private void OnServerButtonClicked()
        {
            Debug.Log("Server button clicked");
            NetworkManager.Singleton.StartServer();
        }
        
        private void OnClientButtonClicked()
        {
            Debug.Log("Client button clicked");
            NetworkManager.Singleton.StartClient();
        }
        
        private void OnHostButtonClicked()
        {
            Debug.Log("Host button clicked");
            NetworkManager.Singleton.StartHost();
        }
    }
}