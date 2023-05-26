using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    [CreateAssetMenu(menuName = "Shakalaka/AppStateMachine/States/Game", fileName = "GameState")]
    public class GameState : AppState
    {
        [SerializeField] private NetworkPlayer playerPrefab;

        private GameScope _gameScope;

        [Inject]
        public void RegisterStateScope(GameScope gameScope)
        {
            _gameScope = gameScope;
        }

        public override async UniTask Enter()
        {
            Debug.Log("Entering GameState...");
            var playerData = _appScope.Container.Resolve<PlayerData>();

            NetworkManager.Singleton.ConnectionApprovalCallback += OnConnectionApproval;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            if (playerData.IsLocal)
            {
                if (playerData.IsHost)
                {
                    NetworkManager.Singleton.GetComponent<UnityTransport>()
                        .SetConnectionData($"127.0.0.1", (ushort)7777);
                    NetworkManager.Singleton.StartHost();
                    var gameSceneProgress = NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
                }
                else if (playerData.IsServer)
                {
                    NetworkManager.Singleton.GetComponent<UnityTransport>()
                        .SetConnectionData($"127.0.0.1", (ushort)7777);
                    NetworkManager.Singleton.StartServer();
                    NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
                }
                else if (playerData.IsClient)
                {
                    NetworkManager.Singleton.GetComponent<UnityTransport>()
                        .SetConnectionData($"127.0.0.1", (ushort)7777);
                    NetworkManager.Singleton.StartClient();
                }
            }
            else if (playerData.IsRelay)
            {
                var authenticator = _appScope.Container.Resolve<Authenticator>();
                await authenticator.Authenticate();
                var relay = _appScope.Container.Resolve<Relay>();

                if (playerData.IsHost)
                {
                    await relay.CreateRelay();
                    NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
                }
                else if (playerData.IsClient)
                    await relay.JoinRelay(playerData.RelayCode);
            }

            NetworkManager.Singleton.SceneManager.OnSynchronize += OnSynchronize;
            NetworkManager.Singleton.SceneManager.OnLoad += OnLoad;
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;


            Debug.Log("Entered GameState...");
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

        public override async UniTask Exit()
        {
            Debug.Log("Exiting GameState...");
        }
    }
}