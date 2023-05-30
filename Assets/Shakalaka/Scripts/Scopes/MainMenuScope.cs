using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class MainMenuScope : LifetimeScope
    {
        [SerializeField] private MainMenuUI ui;

        private Authenticator _authenticator;
        private Relay _relay;

        private CreateTicketResponse _createTicketResponse;
        private float _pollTicketTimer;
        private float _pollTicketTimerMax = 1.5f;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("MainMenuScope Configure");
            builder.RegisterComponent(ui);
        }

        private void Start()
        {
            Debug.Log("MainMenuScope Start");

            _authenticator = Container.Resolve<Authenticator>();
            _relay = Container.Resolve<Relay>();

            var playerData = Container.Resolve<PlayerData>();

            //TODO: move to GameUI and GameScope
            //TODO: subscribe ui to PlayerData to react to changes
            ui.SetPlayerId(playerData.PlayerId);

            ui.LocalServerButtonClicked += () =>
                TrySetupConnection(ConnectionType.Local, ConnectionRole.Server).Forget();

            ui.LocalHostButtonClicked += () =>
                TrySetupConnection(ConnectionType.Local, ConnectionRole.Host).Forget();

            ui.LocalClientButtonClicked += () =>
                TrySetupConnection(ConnectionType.Local, ConnectionRole.Client).Forget();

            ui.RelayHostButtonClicked += () =>
                TrySetupConnection(ConnectionType.Relay, ConnectionRole.Host).Forget();

            ui.RelayClientButtonClicked += (relayCode) =>
                TrySetupConnection(ConnectionType.Relay, ConnectionRole.Client, relayCode).Forget();

            ui.MultiplayerFindMatchButtonClicked += () => OnMultiplayFindMatch().Forget();
            ui.MultiplayerJoinServerButtonClicked += OnMultiplayJoinServer;
        }

        void Update()
        {
            if (_createTicketResponse != null)
            {
                _pollTicketTimer -= Time.deltaTime;
                if (_pollTicketTimer <= 0f)
                {
                    _pollTicketTimer = _pollTicketTimerMax;

                    PollMatchmakerTicket().Forget();
                }
            }
        }

        private async UniTaskVoid PollMatchmakerTicket()
        {
            Debug.Log("Polling matchmaker ticket");

            TicketStatusResponse ticketStatusResponse = await MatchmakerService.Instance.GetTicketAsync(_createTicketResponse.Id);

            if (ticketStatusResponse == null)
                return;

            if (ticketStatusResponse.Type == typeof(MultiplayAssignment))
            {
                MultiplayAssignment multiplayAssignment = ticketStatusResponse.Value as MultiplayAssignment;
                var multiplayAssignmentStatus = multiplayAssignment.Status;
                Debug.Log($"MultiplayAssignment status: {multiplayAssignmentStatus}");

                switch (multiplayAssignmentStatus)
                {
                    case MultiplayAssignment.StatusOptions.Found:
                        _createTicketResponse = null;
                        Debug.Log($"MultiplayAssignment found. Ip: {multiplayAssignment.Ip}. Port: {multiplayAssignment.Port}");
                        string ipv4Address = multiplayAssignment.Ip;
                        ushort port = (ushort)multiplayAssignment.Port;
                        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipv4Address, port);
                        NetworkManager.Singleton.StartClient();
                        break;
                    case MultiplayAssignment.StatusOptions.InProgress:
                        //continue polling
                        break;
                    case MultiplayAssignment.StatusOptions.Failed:
                        _createTicketResponse = null;
                        Debug.Log("MultiplayAssignment Failed!");
                        break;
                    case MultiplayAssignment.StatusOptions.Timeout:
                        _createTicketResponse = null;
                        Debug.Log("MultiplayAssignment Timeout!");
                        break;
                }
            }
        }

        private async UniTaskVoid OnMultiplayFindMatch()
        {
            Debug.Log("Looking for match");
            await _authenticator.Authenticate();

            _createTicketResponse = await MatchmakerService.Instance.CreateTicketAsync(new List<Player>()
            {
                new Player(AuthenticationService.Instance.PlayerId, new MatchmakingPlayerData { Skill = 100 })
            }, new CreateTicketOptions { QueueName = "default-queue" });

            _pollTicketTimer = _pollTicketTimerMax;
        }

        [Serializable]
        public class MatchmakingPlayerData
        {
            public int Skill;
        }

        private void OnMultiplayJoinServer(string ip, ushort port)
        {
            Debug.Log("Setting up Multiplay connection");
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, port);
            NetworkManager.Singleton.StartClient();
        }

        private async UniTaskVoid TrySetupConnection(ConnectionType connectionType, ConnectionRole connectionRole,
            string relayCode = null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += OnConnectionApproval;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

            switch (connectionType)
            {
                case ConnectionType.Local:
                    NetworkManager.Singleton.GetComponent<UnityTransport>()
                        .SetConnectionData($"127.0.0.1", (ushort)7777);
                    switch (connectionRole)
                    {
                        case ConnectionRole.Server:
                            NetworkManager.Singleton.StartServer();
                            NetworkManager.Singleton.SceneManager.LoadScene("PreGame", LoadSceneMode.Single);
                            break;
                        case ConnectionRole.Host:
                            NetworkManager.Singleton.StartHost();
                            NetworkManager.Singleton.SceneManager.LoadScene("PreGame", LoadSceneMode.Single);
                            break;
                        case ConnectionRole.Client:
                            NetworkManager.Singleton.StartClient();
                            break;
                    }

                    break;
                case ConnectionType.Relay:
                    await _authenticator.Authenticate();
                    switch (connectionRole)
                    {
                        case ConnectionRole.Host:
                            await _relay.CreateRelay();
                            NetworkManager.Singleton.SceneManager.LoadScene("PreGame", LoadSceneMode.Single);
                            break;
                        case ConnectionRole.Client:
                            await _relay.JoinRelay(relayCode);
                            break;
                    }

                    break;
            }

            NetworkManager.Singleton.SceneManager.OnSynchronize += OnSynchronize;
            NetworkManager.Singleton.SceneManager.OnLoad += OnLoad;
            NetworkManager.Singleton.SceneManager.OnSceneEvent += OnSceneEvent;
        }

        private void OnSceneEvent(SceneEvent sceneEvent)
        {
            Debug.Log($"OnSceneEvent. {sceneEvent.SceneName} {sceneEvent.SceneEventType}");
        }

        private void OnLoad(ulong clientId, string sceneName, LoadSceneMode loadSceneMode,
            AsyncOperation asyncOperation)
        {
            Debug.Log($"OnLoad. ClientId: {clientId}. SceneName: {sceneName}");
        }

        private void OnSynchronize(ulong clientId)
        {
            Debug.Log($"OnSynchronize. ClientId: {clientId}");
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log($"OnClientConnected. ClientId: {clientId}");
        }

        private void OnConnectionApproval(NetworkManager.ConnectionApprovalRequest request,
            NetworkManager.ConnectionApprovalResponse response)
        {
            Debug.Log($"ConnectionApprovalCallback from ClientNetworkId: {request.ClientNetworkId}");

            response.Approved = true;
            response.CreatePlayerObject = false;
        }
    }
}