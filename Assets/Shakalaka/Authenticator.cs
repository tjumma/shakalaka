using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Shakalaka
{
    public class Authenticator
    {
        private PlayerData _playerData;

        public Authenticator(PlayerData playerData)
        {
            _playerData = playerData;
        }
        
        public async UniTask Authenticate()
        {
            Debug.Log("Authenticating");
            
            var initializationOptions = new InitializationOptions();
            initializationOptions.SetEnvironmentName("development");
            //this profile is not a playerName, its a name of profile from PlayerPrefs on this device so that two builds from the same device can get different PlayerIds and play together.
            var profileName = $"{Random.Range(0, 1000000)}";
            initializationOptions.SetProfile(profileName);
            await UnityServices.InitializeAsync(initializationOptions);

            AuthenticationService.Instance.SignedIn += () =>
            {
                _playerData.PlayerId = AuthenticationService.Instance.PlayerId;
                Debug.Log($"Signed in. PlayerId: {_playerData.PlayerId}");
            };

            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (AuthenticationException e)
            {
                Debug.Log(e);
            }
        }
    }
}