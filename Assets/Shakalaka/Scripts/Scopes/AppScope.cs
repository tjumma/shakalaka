using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class AppScope : LifetimeScope
    {
        [SerializeField] private GameObject quantumConsole;
        [SerializeField] private GameObject eventSystem;
        [SerializeField] private Relay relay;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("AppScope Configure");

            builder.Register<Authenticator>(Lifetime.Singleton);
            builder.Register<PlayerData>(Lifetime.Singleton);
            builder.RegisterComponent(relay);
        }

        private void Start()
        {
            Debug.Log("AppScope Start");

            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(quantumConsole);
            DontDestroyOnLoad(eventSystem);
            DontDestroyOnLoad(relay);

            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}