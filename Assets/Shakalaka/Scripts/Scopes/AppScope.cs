using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class AppScope : LifetimeScope
    {
        [SerializeField] private AppStateMachine appStateMachine;
        [SerializeField] private GameObject quantumConsole;
        [SerializeField] private GameObject eventSystem;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(appStateMachine);
            builder.Register<Authenticator>(Lifetime.Singleton);
            builder.Register<PlayerData>(Lifetime.Singleton);
        }

        void Start()
        {
            appStateMachine.Launch(this).Forget();
            
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(quantumConsole);
            DontDestroyOnLoad(eventSystem);
        }
    }
}