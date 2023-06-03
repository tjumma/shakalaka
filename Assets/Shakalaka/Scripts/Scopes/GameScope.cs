﻿using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Shakalaka
{
    public class GameScope : SingletonScope<GameScope>
    {
        [SerializeField] private ServerBoard serverBoard;
        [SerializeField] private PlayerSpawner playerSpawner;
        [SerializeField] private ClientBoardMVP clientBoardMvp;
        
        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("GameScope Configure");
            builder.RegisterComponent(serverBoard);
            builder.RegisterComponent(playerSpawner);
            builder.RegisterComponent(clientBoardMvp);
        }

        private void Start()
        {
            Debug.Log("GameScope Start");
            
            if (!NetworkManager.Singleton.IsServer)
                return;
            
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
        }

        private void OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted,
            List<ulong> clientsTimedOut)
        {
            playerSpawner.SpawnPlayers();
            serverBoard.GenerateAndSendBoard();
        }
    }
}