using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Systems.Core
{
    public class GameTimer : MonoBehaviour
    {
        [Header("Configuration")] [Tooltip("遊戲總時間(秒)")]
        public float GameMaxTime;

        [Header("State")] public float RemainTime;
        public bool IsUsing;

        [Header("Game Events")] [Tooltip("Trigger when time is up")]
        public UnityEvent OnTimeUp;
        
        [Header("Dual Dependency")] [SerializeField]
        private MicrowaveGameManager _gameManager;

        
        // hook unity cycle
        private event Action OnDestroyed;

        private void Awake()
        {
            IsUsing = false;
            RemainTime = GameMaxTime;
        }

        private void Start()
        {
            _gameManager.onStartGame.AddListener(StartTimer);
            _gameManager.onQuitGame.AddListener(StopTimer);
            _gameManager.onGameOver.AddListener(StopTimer);

            OnDestroyed += () =>
            {
                _gameManager.onStartGame.RemoveListener(StartTimer);
                _gameManager.onQuitGame.RemoveListener(StopTimer);
                _gameManager.onGameOver.RemoveListener(StopTimer);
            };
        }

        private void StartTimer()
        {
            IsUsing = true;
            RemainTime = GameMaxTime;
        }

        private void StopTimer()
        {
            IsUsing = false;
        }

        private void Update()
        {
            if (!IsUsing)
                return;

            RemainTime -= Time.deltaTime;

            if (RemainTime <= 0f)
            {
                TimeUp();
            }
        }

        private void TimeUp()
        {
            RemainTime = 0f;
            IsUsing = false;

            OnTimeUp.Invoke();
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();

            OnDestroyed = null;
        }
    }
}