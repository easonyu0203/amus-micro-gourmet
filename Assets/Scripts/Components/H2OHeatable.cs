using System.Collections.Generic;
using System.Linq;
using Interfaces;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Components
{
    public class H2OHeatable : MonoBehaviour, IHeatable
    {
        [Header("Configuration")] [Tooltip("H2O configurations for different levels")] [SerializeField]
        private List<H2OHeatedConfig> h2OConfigs;

        [FormerlySerializedAs("_maxheatLevel")] [Tooltip("水分子最高受熱等級")] [SerializeField]
        private int _maxHeatLevel = 6;

        [Header("State")] [SerializeField] private int _heatLevel = 1;

        [Header("Dependency")] [SerializeField]
        private SwimmerMovement _swimmerMovement;

        [SerializeField] private Renderer _bodyRenderer;
        [SerializeField] private SkinnedMeshRenderer _faceRenderer;
        private Dictionary<int, H2OHeatedConfig> h2OConfigsDictionary;


        private void Awake()
        {
            // Create a dictionary from the list for efficient lookup
            h2OConfigsDictionary = h2OConfigs.ToDictionary(config => config.LevelId);

            // dependency
            _swimmerMovement = GetComponent<SwimmerMovement>();

            // init state
            ApplyHeatedEffect(_heatLevel);
        }


        public void ReceiveHeat(int calorie)
        {
            _heatLevel += calorie;

            // die when over heat
            if (_heatLevel > _maxHeatLevel)
            {
                // currently just simple delete TODO: die effect
                Destroy(gameObject);
                return;
            }

            // level up heating
            ApplyHeatedEffect(_heatLevel);
        }


        private void ApplyHeatedEffect(int heatLevelId)
        {
            if (!h2OConfigsDictionary.TryGetValue(heatLevelId, out var h2OConfig))
            {
                Debug.LogError("H2OConfig not found for heatLevelId: " + heatLevelId);
                return;
            }

            _swimmerMovement.moveSpeed = h2OConfig.moveSpeed;
            _swimmerMovement.sharpTurnSpeed = h2OConfig.sharpTurnSpeed;
            _swimmerMovement.turnSpeed = h2OConfig.turnSpeed;
            _swimmerMovement.sharpTurnSpeedFactor = h2OConfig.sharpTurnSpeedFactor;
            _swimmerMovement.directionChangeInterval = h2OConfig.directionChangeInterval;

            _bodyRenderer.material = h2OConfig.BodyMaterial;

            // Assuming blend shapes are named or indexed from 0 to 5
            for (var i = 0; i < 6; i++)
            {
                var value = i == heatLevelId - 1
                    ? 100f
                    : 0f; // Setting the corresponding blend shape to 100 and others to 0
                _faceRenderer.SetBlendShapeWeight(i, value);
            }
        }
    }
}