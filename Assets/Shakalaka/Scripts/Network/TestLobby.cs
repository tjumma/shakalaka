using System.Collections.Generic;
using QFSW.QC;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace Shakalaka
{
    public class TestLobby : MonoBehaviour
    {
        [FormerlySerializedAs("testRelay")] [SerializeField] private Relay relay;
        [SerializeField] private float heartbeatTimerMax;
        [SerializeField] [Range(1.1f, 15f)] private float lobbyPollTimerMax;
        
        private Lobby _hostLobby;
        private Lobby _joinedLobby;
        private float _heartbeatTimer;
        private float _lobbyPollTimer;

        private async void Start()
        {
            var initializationOptions = new InitializationOptions();
            initializationOptions.SetEnvironmentName("development");
            //in order for builds on the same pc to have different player ids, you need them to have different profiles
            //initializationOptions.SetProfile(playerName);
            await UnityServices.InitializeAsync(initializationOptions);

            AuthenticationService.Instance.SignedIn += OnSignedIn;
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        private void Update()
        {
            HandleLobbyHeartbeat();
            HandleLobbyPollForUpdates();
        }

        private async void HandleLobbyHeartbeat()
        {
            if (_hostLobby == null)
                return;

            _heartbeatTimer -= Time.deltaTime;
            
            if (_heartbeatTimer <= 0f)
            {
                _heartbeatTimer = heartbeatTimerMax;

                await LobbyService.Instance.SendHeartbeatPingAsync(_hostLobby.Id);
            }
        }

        private async void HandleLobbyPollForUpdates()
        {
            if (_joinedLobby == null)
                return;

            _lobbyPollTimer -= Time.deltaTime;
            
            if (_lobbyPollTimer <= 0f)
            {
                _lobbyPollTimer = lobbyPollTimerMax;

                _joinedLobby = await LobbyService.Instance.GetLobbyAsync(_joinedLobby.Id);

                if (_joinedLobby.Data["RelayCode"].Value != "0")
                {
                    if (_hostLobby == null) //host already joined relay
                    {
                        await relay.JoinRelay(_joinedLobby.Data["RelayCode"].Value);
                        _joinedLobby = null;
                    }
                }
            }
        }

        private void OnSignedIn()
        {
            Debug.Log($"Signed in {AuthenticationService.Instance.PlayerId}");
        }
        
        [Command]
        private async void CreateLobby()
        {
            try
            {
                string lobbyName = "TestLobby";
                int maxPlayers = 4;

                CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions()
                {
                    IsPrivate = false,
                    Player = new Player()
                    {
                        Data = new Dictionary<string, PlayerDataObject>()
                        {
                            {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "tjummaHost")}
                        }
                    },
                    Data = new Dictionary<string, DataObject>()
                    {
                        { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, "CaptureTheFlag", DataObject.IndexOptions.S1)},
                        { "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, "0") }
                    }
                };

                Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers, createLobbyOptions);

                _hostLobby = lobby;
                _joinedLobby = _hostLobby;

                Debug.Log($"Created lobby : {lobby.Name} for {lobby.MaxPlayers} max players. {lobby.Id} {lobby.LobbyCode}");
                PrintPlayers(_hostLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private async void ListLobbies()
        {
            try
            {
                QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions()
                {
                    Count = 25,
                    Filters = new List<QueryFilter>()
                    {
                        new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT),
                        //new QueryFilter(QueryFilter.FieldOptions.S1, "CaptureTheFlag", QueryFilter.OpOptions.EQ)
                    },
                    Order = new List<QueryOrder>()
                    {
                        new QueryOrder(false, QueryOrder.FieldOptions.Created)
                    }
                };
                
                QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync(queryLobbiesOptions);
                var lobbies = queryResponse.Results;
            
                Debug.Log($"Lobbies found: {lobbies.Count}");

                foreach (var lobby in lobbies)
                {
                    Debug.Log($"{lobby.Id}: {lobby.Name} - {lobby.MaxPlayers}max {lobby.Data["GameMode"].Value}");
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private async void JoinLobbyById(string lobbyId)
        {
            try
            {
                var lobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobbyId);
                _joinedLobby = lobby;
                
                Debug.Log($"[Joined lobby {lobby.Id} with code {lobby.LobbyCode}. Players: {lobby.Players.Count}");
                PrintPlayers(lobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
        
        [Command]
        private async void JoinLobbyByCode(string lobbyCode)
        {
            try
            {
                JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions()
                {
                    Player = new Player()
                    {
                        Data = new Dictionary<string, PlayerDataObject>()
                        {
                            {"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "tjummaClient")}
                        }
                    }
                };
                var lobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
                _joinedLobby = lobby;
                
                Debug.Log($"[Joined lobby {lobby.Id} with code {lobby.LobbyCode}. Players: {lobby.Players.Count}");
                PrintPlayers(lobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private async void QuickJoinLobby()
        {
            try
            {
                await LobbyService.Instance.QuickJoinLobbyAsync();
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private void PrintPlayers()
        {
            PrintPlayers(_joinedLobby);
        }

        private void PrintPlayers(Lobby lobby)
        {
            Debug.Log($"Players in lobby {lobby.Name} {lobby.Data["GameMode"].Value}:");
            foreach (var player in lobby.Players)
            {
                Debug.Log($"{player.Id} {player.Data["PlayerName"].Value}");
            }
        }

        [Command]
        private async void UpdateLobbyGameMode(string gameMode)
        {
            try
            {
                _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode) }
                    }
                });

                _joinedLobby = _hostLobby;
                
                PrintPlayers(_hostLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        private async void UpdatePlayerName(string newPlayerName)
        {
            try
            {
                var lobby = await LobbyService.Instance.UpdatePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId,
                    new UpdatePlayerOptions()
                    {
                        Data = new Dictionary<string, PlayerDataObject>()
                        {
                            { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, newPlayerName) }
                        }
                    });

                _joinedLobby = lobby;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private async void LeaveLobby()
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(_joinedLobby.Id, AuthenticationService.Instance.PlayerId);
                _joinedLobby = null;
                _hostLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private async void MigrateLobbyHost()
        {
            try
            {
                _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions()
                {
                    //TODO: check if there is anyone else and what their ids are
                    HostId = _joinedLobby.Players[1].Id
                });

                _joinedLobby = _hostLobby;
                
                PrintPlayers(_hostLobby);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private async void DeleteLobby()
        {
            try
            {
                await LobbyService.Instance.DeleteLobbyAsync(_joinedLobby.Id);
                _hostLobby = null;
                _joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }

        [Command]
        private async void StartGame()
        {
            //can start only if is the host
            if (_hostLobby == null)
                return;

            try
            {
                string relayCode = await relay.CreateRelay();
                
                _hostLobby = await Lobbies.Instance.UpdateLobbyAsync(_hostLobby.Id, new UpdateLobbyOptions()
                {
                    Data = new Dictionary<string, DataObject>()
                    {
                        { "RelayCode", new DataObject(DataObject.VisibilityOptions.Member, relayCode) }
                    }
                });
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }
}