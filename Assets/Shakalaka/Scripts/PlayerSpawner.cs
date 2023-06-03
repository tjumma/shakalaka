﻿using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkPlayer redPlayerPrefab;
        [SerializeField] private NetworkPlayer bluePlayerPrefab;

        [SerializeField] private List<ulong> connectedClientIds = new List<ulong>();

        private IObjectResolver _gameScopeContainer;

        [Inject]
        public void Construct(IObjectResolver gameScopeContainer)
        {
            _gameScopeContainer = gameScopeContainer;
        }

        void Start()
        {
            Debug.Log("PlayerSpawner Start");

            if (!IsServer)
                return;
            
            // connectedClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            // foreach (var clientId in connectedClientIds)
            //     SpawnPlayerObject(clientId);
            
            // NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
            // NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            // NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            // NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void OnLoadEventCompleted(string scenename, LoadSceneMode loadscenemode, List<ulong> clientscompleted, List<ulong> clientstimedout)
        {
            Debug.LogWarning("PlayerSpawner OnLoadEventCompleted");
            connectedClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            // foreach (var clientId in connectedClientIds)
            //     SpawnPlayerObject(clientId);
        }

        public void SpawnPlayers()
        {
            Debug.LogWarning("PlayerSpawner SpawnPlayers");
            connectedClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            foreach (var clientId in connectedClientIds)
                SpawnPlayerObject(clientId);
        }

        private void OnLoadComplete(ulong clientId, string scenename, LoadSceneMode loadscenemode)
        {
            Debug.LogWarning($"PlayerSpawner OnLoadComplete. ClientId: {clientId}");
            
            // connectedClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            // foreach (var clientId in connectedClientIds)
            // SpawnPlayerObject(clientId);
        }

        private void OnClientConnected(ulong clientId)
        {
            Debug.Log("ClientConnected");
            connectedClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
            // SpawnPlayerObject(clientId);
        }
        
        private void OnClientDisconnected(ulong clientId)
        {
            Debug.Log("ClientDisconnected");
            connectedClientIds.Remove(clientId);
            //this shit doesn't get updated instantly for some reason
            // connectedClientIds = new List<ulong>(NetworkManager.Singleton.ConnectedClientsIds);
        }

        public override void OnNetworkSpawn()
        {
            Debug.LogWarning("PlayerSpawner OnNetworkSpawn");
            
            if (!IsServer)
                return;
            
            // NetworkManager.Singleton.SceneManager.OnLoadComplete += OnLoadComplete;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        private void SpawnPlayerObject(ulong clientId)
        {
            var clientIndex = connectedClientIds.IndexOf(clientId);
            
            var playerGo = _gameScopeContainer.Instantiate(
                clientIndex == 0 ? redPlayerPrefab : bluePlayerPrefab,
                Vector3.one * 2 * clientId,
                Quaternion.identity,
                null);
            playerGo.name = $"Player. ClientId: {clientId}";
            playerGo.GetComponent<NetworkObject>().name = $"Player. ClientId: {clientId}";
            playerGo.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        }
    }
}