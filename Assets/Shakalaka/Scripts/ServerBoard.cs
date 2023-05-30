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
            var connectedClientIds = NetworkManager.Singleton.ConnectedClientsIds;
            
            var allCardTypes = new List<int>();
            var numberOfCards = connectedClientIds.Count * 5;
            
            for (int c = 0; c < numberOfCards; c++)
            {
                allCardTypes.Add(c);
            }

            playerHandsByClientId = new Dictionary<ulong, int[]>();

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