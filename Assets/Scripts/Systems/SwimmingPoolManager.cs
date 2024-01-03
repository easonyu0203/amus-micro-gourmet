using System;
using System.Collections.Generic;
using Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems
{
    public class SwimmingPoolManager : MonoBehaviour
    {
        [Header("Dependency")]
        [SerializeField]
        private GameObject _swimmingPoolsParent;

        public SwimmingPool[] SwimmingPools;

        public SwimmingPool GetSwimmingPoolById(int poolId)
        {
            return Array.Find(SwimmingPools, pool => pool.PoolId == poolId);
        }

        private void OnValidate()
        {
            PopulateSwimmingPools();
        }

        private void PopulateSwimmingPools()
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