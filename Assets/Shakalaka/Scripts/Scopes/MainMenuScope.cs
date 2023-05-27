using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuUI ui;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("MainMenuScope Configure");
            builder.RegisterComponent(ui);
        }

        private void Start()
        {
            Debug.Log("MainMenuScope Start");

            var playerData = Container.Resolve<PlayerData>();

            //TODO: subscribe ui to PlayerData to react to changes
            ui.SetPlayerId(playerData.PlayerId);
            
            NetworkManager.Singleton.ConnectionApprovalCallback += OnConnectionApproval;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            ui.LocalServerButtonClicked += () =>
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData($"127.0.0.1", (ushort)7777);
                NetworkManager.Singleton.StartServer();
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            };
            ui.LocalHostButtonClicked += () =>
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData($"127.0.0.1", (ushort)7777);
                NetworkManager.Singleton.StartHost();
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            };
            ui.LocalClientButtonClicked += () =>
            {
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData($"127.0.0.1", (ushort)7777);
                NetworkManager.Singleton.StartClient();
            };

            ui.RelayHostButtonClicked += async () =>
            {
                var authenticator = Container.Resolve<Authenticator>();
                await authenticator.Authenticate();
                var relay = Container.Resolve<Relay>();
                await relay.CreateRelay();
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            };
            ui.RelayClientButtonClicked += async (relayCode) =>
            {
                var authenticator = Container.Resolve<Authenticator>();
                await authenticator.Authenticate();
                var relay = Container.Resolve<Relay>();
                await relay.JoinRelay(relayCode);
            };
            
            //TODO: call these after the NetworkManager has been started
            // NetworkManager.Singleton.SceneManager.OnSynchronize += OnSynchronize;
            // NetworkManager.Singleton.SceneManager.OnLoad += OnLoad;
            // NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            Debug.Log($"OnSceneEvent. {sceneEvent.SceneName} {sceneEvent.SceneEventType}");
        }

        private void OnLoad(ulong clientid, string scenename, LoadSceneMode loadscenemode,
            AsyncOperation asyncoperation)
        {
            Debug.Log($"OnLoad. ClientId: {clientid}. SceneName: {scenename}");
        }

        private void OnSynchronize(ulong clientId)
        {
            Debug.Log($"OnSynchronize. ClientId: {clientId}");
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"OnClientConnected. ClientId: {clientId}");
            // if (NetworkManager.Singleton.IsServer)
            // {
            //     var playerGo = _appScope.Container.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            //     playerGo.name = $"Player. ClientId: {clientId}";
            //     playerGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
            // }
        }

        private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log("ConnectionApprovalCallback");

            response.Approved = true;
            response.CreatePlayerObject = false;
        }
    }
}