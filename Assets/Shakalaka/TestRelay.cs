using System.Threading.Tasks;
using QFSW.QC;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

namespace Shakalaka
{
    public class TestRelay : MonoBehaviour
    {
        // private async void Start()
        // {
        //     var initializationOptions = new InitializationOptions();
        //     initializationOptions.SetEnvironmentName("development");
        //     await UnityServices.InitializeAsync(initializationOptions);
        //
        //     AuthenticationService.Instance.SignedIn += OnSignedIn;
        //     await AuthenticationService.Instance.SignInAnonymouslyAsync();
        // }

        // private void OnSignedIn()
        // {
        //     Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        // }

        [Command]
        public async Task<string> CreateRelay()
        {
            string joinCode = null;
            
            try
            {
                Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

                joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
                Debug.Log(joinCode);

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
        public async void JoinRelay(string joinCode)
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