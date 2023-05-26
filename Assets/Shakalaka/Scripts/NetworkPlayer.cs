using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using VContainer;

namespace Shakalaka
{
    public class NetworkPlayer : NetworkBehaviour
    {
        private ServerBoard _serverBoard;

        [Inject]
        public void Construct(ServerBoard serverBoard)
        {
            _serverBoard = serverBoard;
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
                RequestMySecretIntServerRpc(new ServerRpcParams());
            }
        }

        [ServerRpc]
        private void RequestMySecretIntServerRpc(ServerRpcParams serverRpcParams)
        {
            ulong senderClientId = serverRpcParams.Receive.SenderClientId;
            Debug.Log($"RequestMySecretIntServerRpc. SenderClientId: {senderClientId}");

            var secretInt = _serverBoard.GetSecretIntForClient((int)senderClientId);

            var clientRpcParams = new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> { senderClientId } } };
            SendSecretIntClientRpc(secretInt, clientRpcParams);
        }

        [ClientRpc]
        private void SendSecretIntClientRpc(int secretInt, ClientRpcParams rpsParams)
        {
            Debug.Log($"This will send secretInt only to requestor-owner: {secretInt}");
        }
    }
}