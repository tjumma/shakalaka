using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shakalaka
{
    public class ServerBoard : NetworkBehaviour
    {
        private Dictionary<ulong, int[]> playerHandsByClientId;

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
                return;
            
            Debug.Log($"ServerBoard OnNetworkSpawn. OwnerClientId: {OwnerClientId}");
        }

        public void GenerateAndSendBoard()
        {
            if (!IsServer)
                return;
            
            GenerateServerBoard();
            SendServerBoardToClients();
        }

        private void SendServerBoardToClients()
        {
            foreach (var client in NetworkManager.Singleton.ConnectedClients)
            {
                var clientId = client.Key;
                var clientPlayer = client.Value.PlayerObject.gameObject.GetComponent<NetworkPlayer>();
                
                var playerBoard = GetPlayerBoard(clientId);
                var clientRpcParams = new ClientRpcParams
                    { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { clientId } } };
                clientPlayer.SendPlayerBoardClientRpc(playerBoard, clientRpcParams);
            }
        }

        private void GenerateServerBoard()
        {
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

        public ClientBoard GetPlayerBoard(ulong clientId)
        {
            if (!TryGetOpponentsId(clientId, out var opponentId))
                Debug.LogError($"There is no opponent for client {clientId}!");
            
            var playerBoard = new ClientBoard
            {
                playerPile = new Pile
                {
                    visibility = PileVisibility.VisibleForPlayer,
                    cards = playerHandsByClientId[clientId]
                },
                opponentPile = new Pile()
                {
                    visibility = PileVisibility.InvisibleForPlayer,
                    cards = new int[playerHandsByClientId[opponentId].Length]
                }
            };
            
            return playerBoard;
        }

        private bool TryGetOpponentsId(ulong clientId, out ulong opponentId)
        {
            var connectedClientIds = NetworkManager.Singleton.ConnectedClientsIds;

            foreach (var id in connectedClientIds)
            {
                if (id != clientId)
                {
                    opponentId = id;
                    return true;
                }
            }

            opponentId = ulong.MaxValue;
            return false;
        }
    }
}