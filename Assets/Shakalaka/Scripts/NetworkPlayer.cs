using Unity.Netcode;
using UnityEngine;

namespace Shakalaka
{
    public class NetworkPlayer : NetworkBehaviour
    {
        private NetworkVariable<int> testInt = new NetworkVariable<int>(0);
        private NetworkList<int> cards = new NetworkList<int>();

        private void Start()
        {
            Debug.Log("NetworkPlayer Start");
        }

        public override void OnNetworkSpawn()
        {
            Debug.Log($"NetworkPlayer OnNetworkSpawn. OwnerClientId: {OwnerClientId}");
            Debug.Log($"Current value of testInt is: {testInt.Value}");
            testInt.OnValueChanged += (prevValue, newValue) => { Debug.Log($"testInt value changed from {prevValue} to {newValue}");};
        }

        private void Update()
        {
            if (!IsOwner)
                return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RequestRandomNumberServerRpc(new ServerRpcParams());
            }
        }

        [ServerRpc]
        private void RequestRandomNumberServerRpc(ServerRpcParams serverRpcParams)
        {
            var senderClientId = serverRpcParams.Receive.SenderClientId;
            Debug.Log($"RequestRandomNumberServerRpc. SenderClientId: {senderClientId}");

            if (senderClientId == OwnerClientId)
            {
                Debug.Log("Can change for this player object");
                var randomInt = Random.Range(10, 99);
                Debug.Log($"Changing testInt to {randomInt} on server");
                testInt.Value = randomInt;
            }
            else
            {
                Debug.Log($"Can't change for this player object since its owner is {OwnerClientId}. But request was made by {senderClientId}");
            }
        }
    }
}