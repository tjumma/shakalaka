using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
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
            await SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            var ui = _mainMenuScope.Container.Resolve<MainMenuUI>();
            var playerData = _mainMenuScope.Container.Resolve<PlayerData>();
            
            //TODO: subscribe ui to PlayerData to react to changes
            ui.SetPlayerId(playerData.PlayerId);
            
            ui.LocalHostButtonClicked += () =>
            {
                playerData.IsHost = true;
                playerData.IsLocal = true;
                _sm.TransitionTo(AppStateType.Game).Forget();
            };
            ui.LocalClientButtonClicked += () =>
            {
                playerData.IsClient = true;
                playerData.IsLocal = true;
                _sm.TransitionTo(AppStateType.Game).Forget();
            };
            
            ui.RelayHostButtonClicked += () =>
            {
                playerData.IsHost = true;
                playerData.IsRelay = true;
                _sm.TransitionTo(AppStateType.Game).Forget();
            };
            ui.RelayClientButtonClicked += (relayCode) =>
            {
                playerData.IsClient = true;
                playerData.IsRelay = true;
                playerData.RelayCode = relayCode;
                _sm.TransitionTo(AppStateType.Game).Forget();
            };
        }

        public override async UniTask Exit()
        {
            Debug.Log("Exiting MainMenuState...");
            //await SceneManager.UnloadSceneAsync("MainMenu");
        }
    }
}