using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Systems.Core
{
    /// <summary>
    /// Handle logic to microwave game, the main game loop stuff
    /// </summary>
    public class MicrowaveGameManager : MonoBehaviour
    {
        [Header("State")]
        public bool IsPlaying;
        
        [Header("Game Events")]
        [Tooltip("When the game start")]
        public UnityEvent onStartGame;
        [Tooltip("When quit game")]
        public UnityEvent onQuitGame;
        public UnityEvent onGameOver;
        
        [Header("Dual Dependency")]
        public GameTimer gameTimer;
        public SpawnManager SpawnManager;

        public void StartGame()
        {
            if (IsPlaying)
            {
                Debug.LogWarning("Game already started");
                return;
            }
            
            IsPlaying = true;
            onStartGame.Invoke();
        }

        public void StopGame()
        {
            if (!IsPlaying)
            {
                Debug.LogWarning("Game already stop");
                return;
            }
            
            IsPlaying = false;
            onQuitGame.Invoke();
        }

    }


}