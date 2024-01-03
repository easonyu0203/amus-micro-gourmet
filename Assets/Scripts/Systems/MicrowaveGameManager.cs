using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Systems
{
    public class MicrowaveGameManager : MonoBehaviour
    {
        [Header("Configuration")] [Tooltip("遊戲總時間(秒)")]
        public float GameMaxTime;

        [Tooltip("水分子生成數量權重")] public List<PoolWeights> SwimmingPoolsOccurrencesWeights = new();
        private Dictionary<int,WeightedValue> _poolsWeightsDictionary;

        [Header("Reference")] [SerializeField] private SwimmingPoolManager _swimmingPoolManager;

        [SerializeField] private H2OFactory _h2OFactory;
        
        [Header("State")]
        private Dictionary<SwimmingPool, UnityAction> _poolEventListeners = new Dictionary<SwimmingPool, UnityAction>();


        private void Awake()
        {
            // Convert list to dictionary
            _poolsWeightsDictionary = new Dictionary<int, WeightedValue>();
            foreach (var poolWeight in SwimmingPoolsOccurrencesWeights)
                _poolsWeightsDictionary[poolWeight.poolId] = poolWeight.OccurrenceWeights;
            
            // Check all swimming pools have corresponding weights in dictionary
            foreach (var swimmingPool in _swimmingPoolManager.SwimmingPools)
            {
                if (!_poolsWeightsDictionary.ContainsKey(swimmingPool.PoolId))
                {
                    Debug.LogError($"No corresponding weight found for pool ID {swimmingPool.PoolId}. Please check the configuration.");
                    // continue to the next pool
                    continue;
                }

                // If weight is found, proceed to instantiate H2O
                InstantiateH2OByWeights(swimmingPool.PoolId);
            }
        }

        private void Start()
        {
            RepopulatePoolWhenEmpty();
        }

        private void RepopulatePoolWhenEmpty()
        {
            foreach (SwimmingPool pool in _swimmingPoolManager.SwimmingPools)
            {
                UnityAction action = () => InstantiateH2OByWeights(pool.PoolId);
                _poolEventListeners.Add(pool, action);
                pool.OnPoolEmpty.AddListener(action);
            }
        }

        private void InstantiateH2OByWeights(int poolId)
        {
            var occurrenceWeights = _poolsWeightsDictionary[poolId];
            StartCoroutine(InstantiateH2ODelayed(poolId, occurrenceWeights.GetWeightedValue()));
        }

        private IEnumerator InstantiateH2ODelayed(int poolId, int amount)
        {
            for (var i = 0; i < amount; i++)
            {
                yield return new WaitForSeconds(Random.Range(0f, 1f));
                _h2OFactory.InstantiateH2O(poolId);
            }
        }
        
        private void OnDestroy()
        {
            foreach (var pair in _poolEventListeners)
            {
                pair.Key.OnPoolEmpty.RemoveListener(pair.Value);
            }
        }
    }

    [Serializable]
    public class PoolWeights
    {
        public int poolId;

        [FormerlySerializedAs("weights")] [Tooltip("Reference to the weights")]
        public WeightedValue OccurrenceWeights;
    }
}