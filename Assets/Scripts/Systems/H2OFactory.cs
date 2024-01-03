using Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems
{
    public class H2OFactory : MonoBehaviour
    {
        [SerializeField] private GameObject h2OPrefab;
        [SerializeField] private SwimmingPoolManager swimmingPoolManager;

        /// <summary>
        /// Create a H2O in a pool with random position in pool
        /// </summary>
        /// <param name="poolId"></param>
        public void InstantiateH2O(int poolId)
        {
            SwimmingPool targetPool = swimmingPoolManager.GetSwimmingPoolById(poolId);
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
    }
}