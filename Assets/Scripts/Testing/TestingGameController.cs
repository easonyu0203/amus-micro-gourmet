using Components;
using Systems;
using Systems.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Testing
{
    public class TestingGameController : MonoBehaviour
    {
        [Header("Dependency")] public MicrowaveGameManager gameManager;
        public SwimmingPoolsManager swimmingPoolsManager;

        public void HeatUpPool(int poolId)
        {
            if (!gameManager.IsPlaying)
            {
                Debug.LogWarning("Game haven't start yet");
                return;
            }
            
            SwimmingPool pool = swimmingPoolsManager.GetSwimmingPoolById(poolId);
            pool.PoolHeater.HeatUp();
        }
    }
}