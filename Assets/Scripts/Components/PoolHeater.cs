using System;
using System.Collections.Generic;
using Interfaces;
using Systems;
using UnityEngine;

namespace Components
{

    /// <summary>
    /// Heater for h2O in the pool
    /// </summary>
    public class PoolHeater : MonoBehaviour
    {
        [Header("Dependency")] [SerializeField]
        private SwimmingPool _swimmingPool;

        private void Awake()
        {
            _swimmingPool = GetComponent<SwimmingPool>();
        }

        public void HeatUp(int calories = 1)
        {
            foreach (H2OBehaviour h2O in _swimmingPool.H2Os)
            {
                h2O.Heatable.ReceiveHeat(calories);
            }
        }
    }
}