using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shakalaka
{
    public class ServerBoard : MonoBehaviour
    {
        private Dictionary<ulong, List<int>> _playerHandsByClientId;
        private Dictionary<ulong, List<int>> _playerAreasByClientId;

        public void GenerateAndSendBoard()
        {
            GenerateServerBoard();
            SendBoardToClients();
        }

        private void SendBoardToClients()
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

            _playerHandsByClientId = new Dictionary<ulong, List<int>>();
            _playerAreasByClientId = new Dictionary<ulong, List<int>>();

            foreach (var clientId in connectedClientIds)
            {
                var playerCards = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    var randomIndex = Random.Range(0, allCardTypes.Count);
                    var randomCardType = allCardTypes[randomIndex];
                    playerCards.Add(randomCardType);
                    allCardTypes.RemoveAt(randomIndex);
                }
                _playerHandsByClientId.Add(clientId, playerCards);
                _playerAreasByClientId.Add(clientId, new List<int>());
            }
        }

        public ClientBoardData GetPlayerBoard(ulong clientId)
        {
            if (!TryGetOpponentsId(clientId, out var opponentId))
                Debug.LogError($"There is no opponent for client {clientId}!");
            
            var playerBoard = new ClientBoardData
            {
                playerHandData = new PileData
                {
                    visibility = PileVisibility.VisibleForPlayer,
                    cards = _playerHandsByClientId[clientId].ToArray()
                },
                opponentHandData = new PileData()
                {
                    visibility = PileVisibility.InvisibleForPlayer,
                    cards = new int[_playerHandsByClientId[opponentId].Count]
                },
                playerAreaData = new PileData()
                {
                    visibility = PileVisibility.VisibleForPlayer,
                    cards = _playerAreasByClientId[clientId].ToArray()
                },
                opponentAreaData = new PileData()
                {
                    visibility = PileVisibility.VisibleForPlayer,
                    cards = _playerAreasByClientId[opponentId].ToArray()
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

        public void ProcessCardMoveFromHandToArea(int cardIndex, ulong clientId)
        {
            // ideally we check if the card exists in hand and is permitted to be moved, rejecting the request in case it can't
            
            var cardType = _playerHandsByClientId[clientId][cardIndex];
            _playerHandsByClientId[clientId].RemoveAt(cardIndex);
            _playerAreasByClientId[clientId].Add(cardType);
            
            SendBoardToClients();
        }
    }
}