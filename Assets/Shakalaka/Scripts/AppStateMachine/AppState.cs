using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using VContainer;

namespace Shakalaka
{
    public abstract class AppState : SerializedScriptableObject
    {
        protected AppStateMachine _sm;
        protected AppScope _appScope;

        public void Init(AppStateMachine sm, AppScope appScope)
        {
            _sm = sm;
            _appScope = appScope;
        }

        public abstract UniTask Enter();
        public abstract UniTask Exit();
    }
}