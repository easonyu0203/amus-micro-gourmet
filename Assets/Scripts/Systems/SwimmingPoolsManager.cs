using System;
using System.Collections.Generic;
using Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Systems
{
    public class SwimmingPoolsManager : MonoBehaviour
    {
        [Header("Events")]
        [Tooltip("Trigger when H2O Destroy with <H2OBehavior, pool id>")] 
        public UnityEvent<H2OBehaviour, int> OnH2ODestroy;
        
        [Header("State")]
        public SwimmingPool[] SwimmingPools;
        
        [Header("Dependency")]
        [SerializeField]
        private GameObject _swimmingPoolsParent;
        [SerializeField] private GameObject h2OPrefab;
        
        // unity cycle
        private event Action OnDestroyed;


        public SwimmingPool GetSwimmingPoolById(int poolId)
        {
            return Array.Find(SwimmingPools, pool => pool.PoolId == poolId);
        }
        
        /// <summary>
        /// Create a H2O in a pool with random position in pool
        /// </summary>
        /// <param name="poolId">pool id</param>
        public void InstantiateH2O(int poolId)
        {
            SwimmingPool targetPool = GetSwimmingPoolById(poolId);
            if (targetPool is null)
            {
                Debug.LogError($"Swimming pool with ID {poolId} not found.");
                return;
            }

            GameObject h2OObject = Instantiate(
                h2OPrefab,
                targetPool.GetRandomPoint(),
                h2OPrefab.transform.rotation, 
                targetPool.transform
            );

            H2OBehaviour h2O = h2OObject.GetComponent<H2OBehaviour>();
            
            // register to pool
            targetPool.RegisterH2O(h2O);
            
            // set trigger for OnH2ODestroy
            UnityAction<H2OBehaviour> triggerH2ODestroyedAction = (innerH2O) => OnH2ODestroy.Invoke(innerH2O, poolId); 
            h2O.OnDestroyed.AddListener(triggerH2ODestroyedAction);
            OnDestroyed += () => h2O.OnDestroyed.RemoveListener(triggerH2ODestroyedAction);
        }

        /// <summary>
        /// remove all the H2O from all pool
        /// </summary>
        public void cleanAllPool()
        {
            foreach (SwimmingPool pool in SwimmingPools)
            {
                pool.CleanPool();
            }
        }

        private void OnValidate()
        {
            PopulateSwimmingPoolsFromParent();
        }

        private void PopulateSwimmingPoolsFromParent()
        {
            if (_swimmingPoolsParent != null)
            {
                // Get all SwimmingPool components on the children of the specified parent
                SwimmingPools = _swimmingPoolsParent.GetComponentsInChildren<SwimmingPool>(true);
            }
            else
            {
                Debug.LogWarning("SwimmingPoolsParent is not set in the editor.");
                SwimmingPools = Array.Empty<SwimmingPool>(); // Reset the array
            }
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
            OnDestroyed = null;
        }
    }
}