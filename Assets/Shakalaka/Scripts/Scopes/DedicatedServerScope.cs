using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Multiplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class DedicatedServerScope : LifetimeScope
    {
        [SerializeField] private Relay relay;
        private IServerQueryHandler _serverQueryHandler;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(relay);
            builder.Register<Authenticator>(Lifetime.Singleton);
            builder.Register<PlayerData>(Lifetime.Singleton);
        }

        private async UniTask Start()
        {
            Debug.Log("DedicatedServerScope Start");
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(relay);

            NetworkManager.Singleton.ConnectionApprovalCallback += OnConnectionApproval;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            var initializationOptions = new InitializationOptions();
            initializationOptions.SetEnvironmentName("development");
            await UnityServices.InitializeAsync(initializationOptions);

            MultiplayEventCallbacks multiplayEventCallbacks = new MultiplayEventCallbacks();
            multiplayEventCallbacks.Allocate += (allocation) => MultiplayEventCallbacks_Allocate(allocation).Forget();
            multiplayEventCallbacks.Deallocate += MultiplayEventCallbacks_Deallocate;
            multiplayEventCallbacks.Error += MultiplayEventCallbacks_Error;
            multiplayEventCallbacks.SubscriptionStateChanged += MultiplayEventCallbacks_SubscriptionStateChanged;

            IServerEvents serverEvents =
                await MultiplayService.Instance.SubscribeToServerEventsAsync(multiplayEventCallbacks);
            _serverQueryHandler =
                await MultiplayService.Instance.StartServerQueryHandlerAsync(4, "MyTestServer", "Shakalaka",
                    "MyBuildId", "MyMap");

            var serverConfig = MultiplayService.Instance.ServerConfig;
            if (serverConfig.AllocationId != "")
                //already allocated
                MultiplayEventCallbacks_Allocate(new MultiplayAllocation("", serverConfig.ServerId,
                    serverConfig.AllocationId)).Forget();


            // var authenticator = Container.Resolve<Authenticator>();
            // await authenticator.Authenticate();
            // await relay.CreateRelayServer();
            // NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            // _serverQueryHandler =
            //     await MultiplayService.Instance.StartServerQueryHandlerAsync(4, "MyTestServer", "Shakalaka",
            //         "MyBuildId", "MyMap");
            // await MultiplayService.Instance.ReadyServerForPlayersAsync();
        }

        private bool _alreadyAutoAllocated;

        private async UniTaskVoid MultiplayEventCallbacks_Allocate(MultiplayAllocation allocation)
        {
            Debug.Log("MultiplayEventCallbacks_Allocate");

            if (_alreadyAutoAllocated)
            {
                Debug.Log("Already auto allocated!");
                return;
            }

            _alreadyAutoAllocated = true;

            var serverConfig = MultiplayService.Instance.ServerConfig;
            Debug.Log($"Server ID[{serverConfig.ServerId}]");
            Debug.Log($"AllocationID[{serverConfig.AllocationId}]");
            Debug.Log($"Port[{serverConfig.Port}]");
            Debug.Log($"QueryPort[{serverConfig.QueryPort}");
            Debug.Log($"LogDirectory[{serverConfig.ServerLogDirectory}]");

            string ipv4Address = "0.0.0.0";
            ushort port = serverConfig.Port;
            
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipv4Address, port, "0.0.0.0");
            NetworkManager.Singleton.StartServer();
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            await MultiplayService.Instance.ReadyServerForPlayersAsync();
        }

        private void MultiplayEventCallbacks_Deallocate(MultiplayDeallocation deallocation)
        {
            Debug.Log("MultiplayEventCallbacks_Deallocate");
        }

        private void MultiplayEventCallbacks_Error(MultiplayError error)
        {
            Debug.Log("MultiplayEventCallbacks_Error");
        }

        private void MultiplayEventCallbacks_SubscriptionStateChanged(
            MultiplayServerSubscriptionState subscriptionState)
        {
            Debug.Log("MultiplayEventCallbacks_SubscriptionStateChanged");
        }

        void Update()
        {
            if (_serverQueryHandler != null)
            {
                _serverQueryHandler.CurrentPlayers = (ushort)NetworkManager.Singleton.ConnectedClientsIds.Count;
                _serverQueryHandler.UpdateServerCheck();
            }
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"OnClientConnected. ClientId: {clientId}");
        }

        private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log($"ConnectionApprovalCallback from ClientNetworkId: {request.ClientNetworkId}");

            response.Approved = true;
            response.CreatePlayerObject = false;
        }
    }
}