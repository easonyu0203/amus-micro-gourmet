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
    public class SpawnManager : MonoBehaviour
    {
        [Header("Configuration")]

        [Tooltip("水分子生成數量權重")] public List<PoolWeights> swimmingPoolsOccurrencesWeights = new();
        private Dictionary<int, WeightedValue> _poolsWeightsDictionary;
        
        [Header("State")]
        private readonly Dictionary<SwimmingPool, UnityAction> _poolRefillActions = new ();


        [Header("Dual Dependency")] [SerializeField]
        private MicrowaveGameManager gameManager;
        
         [Header("Dependency")] [SerializeField] private SwimmingPoolsManager swimmingPoolsManager;
        
        // hook unity cycle
        private event Action OnDestroyed;
        
        private void Awake()
        {
            // Convert list to dictionary
            _poolsWeightsDictionary = new Dictionary<int, WeightedValue>();
            foreach (var poolWeight in swimmingPoolsOccurrencesWeights)
                _poolsWeightsDictionary[poolWeight.poolId] = poolWeight.occurrenceWeights;

            // Check all swimming pools have corresponding weights in dictionary
            foreach (var swimmingPool in swimmingPoolsManager.SwimmingPools)
            {
                if (!_poolsWeightsDictionary.ContainsKey(swimmingPool.PoolId))
                {
                    Debug.LogError(
                        $"No corresponding weight found for pool ID {swimmingPool.PoolId}. Please check the configuration.");
                }
            }
        }

        private void Start()
        {
            gameManager.onStartGame.AddListener(OnStartGame);
            gameManager.onQuitGame.AddListener(OnStopGame);
            gameManager.onGameOver.AddListener(OnStopGame);

            OnDestroyed += () =>
            {
                gameManager.onStartGame.RemoveListener(OnStartGame);
                gameManager.onQuitGame.RemoveListener(OnStopGame);
                gameManager.onGameOver.RemoveListener(OnStopGame);
            };
        }

        private void OnStartGame()
        {
            InitFillPools();
            RefillPoolsHook();
        }
        
        private void OnStopGame()
        {
            UnhookRefillPools();
            CleanPools();
        }

        private void InitFillPools()
        {
            // instantiate H2O for each pool
            foreach (var swimmingPool in swimmingPoolsManager.SwimmingPools)
            {
                FillPool(swimmingPool.PoolId);
            }
        }
        
        /// <summary>   
        /// hook to event which enable refill pool whenever pool is empty
        /// </summary>
        private void RefillPoolsHook()
        {
            foreach (SwimmingPool pool in swimmingPoolsManager.SwimmingPools)
            {
                UnityAction fillPoolAction = () => FillPool(pool.PoolId);
                _poolRefillActions[pool] = fillPoolAction;

                pool.OnPoolEmpty.AddListener(fillPoolAction);
            }
        }
        
        private void UnhookRefillPools()
        {
            foreach (var entry in _poolRefillActions)
            {
                entry.Key.OnPoolEmpty.RemoveListener(entry.Value);
            }

            _poolRefillActions.Clear();
        }

        private void CleanPools()
        {
            foreach (var swimmingPool in swimmingPoolsManager.SwimmingPools)
            {
                swimmingPool.CleanPool();
            }
        }
        
        /// <summary>
        /// fill a given pool with randomize create time for each H2O to have the H2O casually pop out effect
        /// </summary>
        /// <param name="poolId">pool id</param>
        private void FillPool(int poolId)
        {
            var occurrenceWeights = _poolsWeightsDictionary[poolId];
            StartCoroutine(InstantiateH2ODelayed(poolId, occurrenceWeights.GetWeightedValue()));

            IEnumerator InstantiateH2ODelayed(int poolId, int amount)
            {
                for (var i = 0; i < amount; i++)
                {
                    yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));
                    swimmingPoolsManager.InstantiateH2O(poolId);
                }
            }
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
            UnhookRefillPools(); // Ensure to unhook when the object is destroyed
            OnDestroyed = null;
        }
    }
    
    /// <summary>
    /// contain the occurrence weights for the pool, the occurrence weight determine the probability
    /// of how much H2O will be create when fill a pool
    /// </summary>
    [Serializable]
    public class PoolWeights
    {
        public int poolId;

        [Tooltip("Reference to the weights")]
        public WeightedValue occurrenceWeights;
    }
}