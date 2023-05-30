using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class GameScope : SingletonScope<GameScope>
    {
        [SerializeField] private ServerBoard serverBoard;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private CardSpawner cardsSpawner;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameScope Configure");
            builder.RegisterComponent(serverBoard);
            builder.RegisterComponent(playerSpawner);
            builder.RegisterComponent(cardsSpawner);
        }

        private void Start()
        {
            Debug.Log("GameScope Start");
        }
    }
}