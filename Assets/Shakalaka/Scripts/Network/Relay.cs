using Cysharp.Threading.Tasks;
using QFSW.QC;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Shakalaka
{
    public class Relay : MonoBehaviour
    {
        [Command]
        public async UniTask<string> CreateRelay()
        {
            string joinCode = null;
            
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

                joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log($"Join code: {joinCode}");

                RelayServerData relayServerData = new (allocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartHost();
            }
            catch (RelayServiceException exception)
            {
                Debug.Log(exception);
            }
            
            return joinCode;
        }

        [Command]
        public async UniTask JoinRelay(string joinCode)
        {
            try
            {
                Debug.Log($"Joining Relay with code: {joinCode}");
                JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

                RelayServerData relayServerData = new (joinAllocation, "dtls");
                NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
                NetworkManager.Singleton.StartClient();
            }
            catch (RelayServiceException exception)
            {
                Debug.Log(exception);
            }
        }
    }
}