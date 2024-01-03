using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    public class SwimmerMovement : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Swimming speed of the swimmer.")]
        public float moveSpeed = 2f;
    
        [Tooltip("Sharp turning speed of the swimmer in degrees.")]
        public float sharpTurnSpeed = 300f;

        [Tooltip("Turning speed of the swimmer in degrees.")]
        public float turnSpeed = 300f;

        [Tooltip("Factor to reduce speed during sharp turns.")]
        public float sharpTurnSpeedFactor = 0.1f;

        [Tooltip("Interval in seconds for changing direction.")]
        public float directionChangeInterval = 1f;

        [Header("States")]
        [SerializeField] private float _nextDirectionChangeTime = 0f;
        [SerializeField] private Vector2 _movementDirection;
        [SerializeField] private bool _recentlyTurned = false;
        
        [Header("Dependency")]
        [Tooltip("The pool this swimmer live in (WARNING: should put as a child of a SwimmingPool")]
        [SerializeField]
        private SwimmingPool _swimmingPool;

        private void Awake()
        {
            _swimmingPool = GetComponentInParent<SwimmingPool>();
        }

        private void Start()
        {
            CalculateNewDirection();
        }

        private void Update()
        {
            if (Time.time >= _nextDirectionChangeTime)
            {
                CalculateNewDirection();
            }

            if (IsNearEdge() && !_recentlyTurned)
            {
                CalculateNewDirection();
                _recentlyTurned = true;
            }
            else if (!IsNearEdge() && _recentlyTurned)
            {
                CalculateNewDirection();
                _recentlyTurned = false;
            }

            MoveSwimmer();
        }

        void CalculateNewDirection()
        {
            _nextDirectionChangeTime = Time.time + directionChangeInterval;

            if (IsNearEdge())
            {
                Vector2 toCenter = (_swimmingPool.PoolCenter - (Vector2)transform.position).normalized;
                _movementDirection = toCenter;
            }
            else
            {
                float randomAngle = Random.Range(-60, 60);
            
                _movementDirection = RotateVector2(_movementDirection, randomAngle);
            }
        }

        void MoveSwimmer()
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, _movementDirection);
            Quaternion currentRotation = transform.rotation;

            // Calculate the angle difference between current direction and target direction
            float angleDifference = Quaternion.Angle(currentRotation, targetRotation);

            // Adjust speed based on the angle difference
            float currentSpeed = moveSpeed;
            if (angleDifference > 10f && IsNearEdge()) 
            {
                currentSpeed *= sharpTurnSpeedFactor;
            }

            float currentTurnSpeed = IsNearEdge() ? sharpTurnSpeed : turnSpeed;

            // Smooth turning
            transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, currentTurnSpeed * Time.deltaTime);

            // Forward movement with adjusted speed
            transform.position += transform.up * (currentSpeed * Time.deltaTime);
        }

        bool IsNearEdge()
        {
            return ((Vector2)transform.position - _swimmingPool.PoolCenter).sqrMagnitude > _swimmingPool.PoolRadius * _swimmingPool.PoolRadius;
        }
    
        private Vector2 RotateVector2(Vector2 vector, float degree)
        {
            float rad = degree * Mathf.Deg2Rad;
            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);
    
            float tx = vector.x;
            float ty = vector.y;
            return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
        }
    }
}