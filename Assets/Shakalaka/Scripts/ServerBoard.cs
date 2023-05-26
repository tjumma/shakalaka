using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Shakalaka
{
    public class ServerBoard : NetworkBehaviour
    {
        private Dictionary<int, int> secretIntsByClientId;

        public override void OnNetworkSpawn()
        {
            Debug.Log($"ServerBoard OnNetworkSpawn. OwnerClientId: {OwnerClientId}");

            secretIntsByClientId = new Dictionary<int, int>()
            {
                { 1, 42 },
                { 2, 69 }
            };
        }

        public int GetSecretIntForClient(int clientId)
        {
            return secretIntsByClientId[clientId];
        }
    }
}