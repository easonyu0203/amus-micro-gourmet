using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "H2OHeatedConfig", menuName = "H2O/heatedConfig", order = 1)]
    public class H2OHeatedConfig : ScriptableObject
    {
        [Tooltip("水分子加熱等級")]
        public int LevelId = 1;

        [Tooltip("H2O material for this level")]
        public Material BodyMaterial;
        
        [Tooltip("Movement speed of the H2O.")]
        public float moveSpeed = 2f;

        [Tooltip("Sharp turning speed of the H2O in degrees.")]
        public float sharpTurnSpeed = 300f;

        [Tooltip("Turning speed of the H2O in degrees.")]
        public float turnSpeed = 300f; 

        [Tooltip("Factor to reduce speed during sharp turns.")]
        public float sharpTurnSpeedFactor = 0.1f;

        [Tooltip("Interval in seconds for changing direction.")]
        public float directionChangeInterval = 1f;
    }
}