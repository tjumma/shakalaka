using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Shakalaka
{
    [CreateAssetMenu(menuName = "Shakalaka/AppStateMachine/States/Game", fileName = "GameState")]
    public class GameState : AppState
    {
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
            
            if (playerData.IsLocal)
            {
                //TODO: set transport to UnityTransport!

                if (playerData.IsHost)
                {
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData($"127.0.0.1", (ushort)7777);
                    NetworkManager.Singleton.StartHost();
                    NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
                }
                else if (playerData.IsClient)
                {
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData($"127.0.0.1", (ushort)7777);
                    NetworkManager.Singleton.StartClient();
                }
            }
            else if (playerData.IsRelay)
            {
                //TODO: set transport to RelayUnityTransport!
                
                var authenticator = _appScope.Container.Resolve<Authenticator>();
                await authenticator.Authenticate();
                var relay = _gameScope.Container.Resolve<Relay>();
            
                if (playerData.IsHost)
                {
                    relay.CreateRelay().Forget();
                    NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
                }
                else if (playerData.IsClient)
                    relay.JoinRelay(playerData.RelayCode).Forget();
            }
        }
        
        public override async UniTask Exit()
        {
            Debug.Log("Exiting GameState...");
        }
    }
}