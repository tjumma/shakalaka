using Cysharp.Threading.Tasks;
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
            await SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
            var relay = _gameScope.Container.Resolve<Relay>();
            var playerData = _gameScope.Container.Resolve<PlayerData>();
            
            if (playerData.IsHost)
                relay.CreateRelay().Forget();
            else if (playerData.IsClient)
                relay.JoinRelay(playerData.RelayCode).Forget();
        }
        
        public override async UniTask Exit()
        {
            Debug.Log("Exiting GameState...");
        }
    }
}