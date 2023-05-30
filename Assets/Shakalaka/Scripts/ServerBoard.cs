using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shakalaka
{
    public class ServerBoard : NetworkBehaviour
    {
        private Dictionary<ulong, int[]> playerHandsByClientId;

        public override void OnNetworkSpawn()
        {
            Debug.Log($"ServerBoard OnNetworkSpawn. OwnerClientId: {OwnerClientId}");

            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
        {
            if (!IsServer)
                return;
            
            Debug.Log($"OnLoadEventCompleted. ClientsConnected: {NetworkManager.Singleton.ConnectedClientsIds.Count}");
            var allCardTypes = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            playerHandsByClientId = new Dictionary<ulong, int[]>();
            var connectedClientIds = NetworkManager.Singleton.ConnectedClientsIds;

            foreach (var clientId in connectedClientIds)
            {
                var playerCards = new int[5];
                for (int i = 0; i < 5; i++)
                {
                    var randomIndex = Random.Range(0, allCardTypes.Count);
                    var randomCardType = allCardTypes[randomIndex];
                    playerCards[i] = randomCardType;
                    allCardTypes.RemoveAt(randomIndex);
                }
                playerHandsByClientId.Add(clientId, playerCards);
            }
        }

        public int[] GetPlayerHand(ulong clientId)
        {
            return playerHandsByClientId[clientId];
        }
    }
}