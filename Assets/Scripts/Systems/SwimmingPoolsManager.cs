using System;
using System.Collections.Generic;
using Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems
{
    public class SwimmingPoolsManager : MonoBehaviour
    {
        [Header("State")]
        public SwimmingPool[] SwimmingPools;
        
        [Header("Dependency")]
        [SerializeField]
        private GameObject _swimmingPoolsParent;
        [SerializeField] private GameObject h2OPrefab;


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
    }
}