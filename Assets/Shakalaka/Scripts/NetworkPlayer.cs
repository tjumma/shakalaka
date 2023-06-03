using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Shakalaka
{
    public class NetworkPlayer : NetworkBehaviour
    {
        private ServerBoard _serverBoard;
        private ClientBoardMVP _clientBoardMvp;

        [Inject]
        public void Construct(ServerBoard serverBoard, ClientBoardMVP clientBoardMvp)
        {
            Debug.Log("NetworkPlayer Construct");
            _serverBoard = serverBoard;
            _clientBoardMvp = clientBoardMvp;
        }
        
        private void Start()
        {
            Debug.Log("NetworkPlayer Start");
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log($"NetworkPlayer OnNetworkSpawn. OwnerClientId: {OwnerClientId}");
            GameScope.Instance.Container.Inject(this);
        }

        private void Update()
        {
            if (!IsOwner) //local player object or object owned by local player
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RequestPlayerBoardServerRpc(new ServerRpcParams());
            }
        }

        [ServerRpc]
        private void RequestPlayerBoardServerRpc(ServerRpcParams serverRpcParams)
        {
            ulong senderClientId = serverRpcParams.Receive.SenderClientId;
            Debug.Log($"RequestPlayerBoardServerRpc. SenderClientId: {senderClientId}");
            
            var playerBoard = _serverBoard.GetPlayerBoard(senderClientId);
            var clientRpcParams = new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { senderClientId } } };
            SendPlayerBoardClientRpc(playerBoard, clientRpcParams);
        }
        
        [ClientRpc]
        public void SendPlayerBoardClientRpc(ClientBoard clientBoard, ClientRpcParams rpsParams)
        {
            Debug.Log($"This will send specific PlayerBoard to each player");
            
            _clientBoardMvp.SetupBoard(clientBoard);
        }
    }
}