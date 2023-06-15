using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class GameScope : SingletonScope<GameScope>
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ServerBoard serverBoard;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private ClientBoardMVP clientBoardMvp;
        [SerializeField] private CardSelector cardSelector;

        public NetworkPlayer Player { get; private set; }
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameScope Configure");
            builder.RegisterComponent(mainCamera);
            builder.RegisterComponent(serverBoard);
            builder.RegisterComponent(playerSpawner);
            builder.RegisterComponent(clientBoardMvp);
            builder.RegisterComponent(cardSelector);
        }

        private void Start()
        {
            Debug.Log("GameScope Start");
            
            if (!NetworkManager.Singleton.IsServer)
                return;
            
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted,
            List<ulong> clientsTimedOut)
        {
            playerSpawner.SpawnPlayers();
            serverBoard.GenerateAndSendBoard();
        }

        public void RegisterPlayer(NetworkPlayer player)
        {
            Player = player;
            Container.Inject(player);
        }
    }
}