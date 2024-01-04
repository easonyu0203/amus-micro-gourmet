using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Components
{
    /// <summary>
    /// The SwimmingPool class represents a swimming area for a swimmer in the game.
    /// This class holds data related to the center and size (radius) of the swimming 
    /// </summary>
    public class SwimmingPool : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private int _poolId;
     
        [SerializeField]
        private float poolRadius = 10f;
        
        [Header("Events")]
        [Tooltip("trigger when no more h2O in the pool")]
        public UnityEvent OnPoolEmpty = new ();

        [Header("Dependency")] public PoolHeater PoolHeater;
        
        [Header("State")] [Tooltip("H2Os in this pool")]  public List<H2OBehaviour> H2Os;
        
        // hook to unity cycle
        private event Action OnDestroyed;
        
        public int PoolId => _poolId;

        public float PoolRadius => poolRadius;

        public Vector2 PoolCenter => transform.position;

        public void RegisterH2O(H2OBehaviour h2O)
        {
            H2Os.Add(h2O);

            UnityAction onH2ODestroyedAction = () =>
            {
                H2Os.Remove(h2O);
                if (H2Os.Count == 0)
                {
                    OnPoolEmpty.Invoke();
                }
            };

            h2O.OnDestroyed.AddListener(onH2ODestroyedAction);
            OnDestroyed += () => h2O.OnDestroyed.RemoveListener(onH2ODestroyedAction);
        }

        /// <summary>
        /// remove all H2O from the pool
        /// </summary>
        public void CleanPool()
        {
            foreach (H2OBehaviour h2O in H2Os)
            {
                Destroy(h2O.gameObject);
            }
        }

        /// <summary>
        /// Gets a random point within the swimming pool.
        /// </summary>
        /// <returns>A random point within the pool's radius.</returns>
        public Vector2 GetRandomPoint()
        {
            float angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            float radius = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * poolRadius;

            float x = PoolCenter.x + radius * Mathf.Cos(angle);
            float y = PoolCenter.y + radius * Mathf.Sin(angle);

            return new Vector2(x, y);
        }
     
        /// <summary>
        /// draw the swimming area in the Unity editor for easy visualization.
        /// </summary>
        void OnDrawGizmos()
        {
            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position, Vector3.forward, poolRadius);
        }
        
        private void OnDestroy()
        {
            OnDestroyed?.Invoke();
            
            OnPoolEmpty.RemoveAllListeners();
        }
    }
}