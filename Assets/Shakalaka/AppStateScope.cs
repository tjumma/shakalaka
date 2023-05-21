using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public abstract class AppStateScope : LifetimeScope
    {
        [SerializeField] private AppState appState;

        protected override void Awake()
        {
            base.Awake();

            Debug.Log("AppStateScope Awake");

            // we can also do it from the AppStateMachine itself via Parent.Container.Resolve<AppStateMachine>()
            if (appState!=null)
                Container.Inject(appState);
            else
                Debug.LogError($"{name}'s state is null!");
        }
    }
}