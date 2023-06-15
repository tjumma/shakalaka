using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private NetworkPlayer redPlayerPrefab;
        [SerializeField] private NetworkPlayer bluePlayerPrefab;

        [SerializeField] private List<ulong> connectedClientIds = new List<ulong>();

        private IObjectResolver _gameScopeContainer;

        [Inject]
        public void Construct(IObjectResolver gameScopeContainer)
        {
            _gameScopeContainer = gameScopeContainer;
        }

        public void SpawnPlayers()
        {
            Debug.LogWarning("PlayerSpawner SpawnPlayers");
            connectedClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            foreach (var clientId in connectedClientIds)
                SpawnPlayerObject(clientId);
        }

        private void SpawnPlayerObject(ulong clientId)
        {
            var clientIndex = connectedClientIds.IndexOf(clientId);
            
            var playerGo = _gameScopeContainer.Instantiate(
                clientIndex == 0 ? redPlayerPrefab : bluePlayerPrefab,
                Vector3.one * 2 * clientId,
                Quaternion.identity,
                null);
            playerGo.name = $"Player. ClientId: {clientId}";
            playerGo.GetComponent<NetworkObject>().name = $"Player. ClientId: {clientId}";
            playerGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
}