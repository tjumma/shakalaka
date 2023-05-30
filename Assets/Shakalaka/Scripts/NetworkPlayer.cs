using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Shakalaka
{
    public class NetworkPlayer : NetworkBehaviour
    {
        private ServerBoard _serverBoard;
        private CardSpawner _cardSpawner;

        [Inject]
        public void Construct(ServerBoard serverBoard, CardSpawner cardSpawner)
        {
            Debug.Log("NetworkPlayer Construct");
            _serverBoard = serverBoard;
            _cardSpawner = cardSpawner;
        }
        
        private void Start()
        {
            Debug.Log("NetworkPlayer Start");
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log($"NetworkPlayer OnNetworkSpawn. OwnerClientId: {OwnerClientId}");
        }

        private void Update()
        {
            if (!IsOwner) //local player object or object owned by local player
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RequestPlayerHandServerRpc(new ServerRpcParams());
            }
        }

        [ServerRpc]
        private void RequestPlayerHandServerRpc(ServerRpcParams serverRpcParams)
        {
            ulong senderClientId = serverRpcParams.Receive.SenderClientId;
            Debug.Log($"RequestPlayerHandServerRpc. SenderClientId: {senderClientId}");

            var playerHand = _serverBoard.GetPlayerHand(senderClientId);

            var clientRpcParams = new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { senderClientId } } };
            SendPlayerHandClientRpc(playerHand, clientRpcParams);
        }

        [ClientRpc]
        private void SendPlayerHandClientRpc(int[] playerHand, ClientRpcParams rpsParams)
        {
            Debug.Log($"This will send player hand only to hand owner");

            _cardSpawner.FillPlayerHand(playerHand);
        }
    }
}