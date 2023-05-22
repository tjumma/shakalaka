using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Shakalaka
{
    [CreateAssetMenu(menuName = "Shakalaka/AppStateMachine/States/MainMenu", fileName = "MainMenuState")]
    public class MainMenuState : AppState
    {
        private MainMenuScope _mainMenuScope;

        [Inject]
        public void RegisterStateScope(MainMenuScope mainMenuScope)
        {
            _mainMenuScope = mainMenuScope;
        }

        public override async UniTask Enter()
        {
            Debug.Log("Entering MainMenuState...");
            await SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Additive);
            var ui = _mainMenuScope.Container.Resolve<MainMenuUI>();
            var playerData = _mainMenuScope.Container.Resolve<PlayerData>();
            ui.SetPlayerId(playerData.PlayerId);

            ui.HostButtonClicked += () =>
            {
                playerData.IsHost = true;
                _sm.TransitionTo(AppStateType.Game).Forget();
            };
            ui.JoinButtonClicked += (relayCode) =>
            {
                playerData.IsClient = true;
                playerData.RelayCode = relayCode;
                _sm.TransitionTo(AppStateType.Game).Forget();
            };
        }

        public override async UniTask Exit()
        {
            Debug.Log("Exiting MainMenuState...");
            await SceneManager.UnloadSceneAsync("MainMenu");
        }
    }
}