using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class PreGameScope : LifetimeScope
    {
        [SerializeField] private PreGameUI ui;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("PreGameScope Configure");
        }

        private void Start()
        {
            Debug.Log("PreGameScope Start");

            if (!NetworkManager.Singleton.IsServer)
                return;

            if (NetworkManager.Singleton.ConnectedClients.Count == 3)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
            else
            {
                ui.SetPlayersConnected(NetworkManager.Singleton.ConnectedClients.Count);
            
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            }
        }

        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log($"PreGame OnClientDisconnected. ClientId: {clientId}");
            ui.SetPlayersConnected(NetworkManager.Singleton.ConnectedClients.Count);
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"PreGame OnClientConnected. ClientId: {clientId}");
            if (NetworkManager.Singleton.ConnectedClients.Count == 3)
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
            }
            else
            {
                ui.SetPlayersConnected(NetworkManager.Singleton.ConnectedClients.Count);
            }
        }
    }
}