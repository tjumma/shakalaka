using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class GameScope : LifetimeScope
    {
        [SerializeField] private ServerBoard serverBoard;
        [SerializeField] private PlayerSpawner playerSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameScope Configure");
            builder.RegisterComponent(serverBoard);
            builder.RegisterComponent(playerSpawner);
        }

        private void Start()
        {
            Debug.Log("GameScope Start");
        }
    }
}