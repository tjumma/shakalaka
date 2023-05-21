using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Shakalaka
{
    [CreateAssetMenu(menuName = "Shakalaka/AppStateMachine/States/Startup", fileName = "StartupState")]
    public class StartupState : AppState
    {
        public override async UniTask Enter()
        {
            Debug.Log("Entering StartupState...");
            var authenticator = _appScope.Container.Resolve<Authenticator>();
            await authenticator.Authenticate();
            _sm.TransitionTo(AppStateType.MainMenu).Forget();
        }
        
        public override async UniTask Exit()
        {
            Debug.Log("Exiting StartupState...");
        }
    }
}