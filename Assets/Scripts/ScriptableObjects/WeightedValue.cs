using System.Collections.Generic;
using UnityEngine;

namespace ScriptableObjects
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "WeightedValue", menuName = "Probability/WeightedValue", order = 1)]
    public class WeightedValue : ScriptableObject
    {
        [System.Serializable]
        public struct ValueWeightPair
        {
            public int value;
            public float weight;
        }

        public List<ValueWeightPair> valueWeightPairs;

        public int GetWeightedValue()
        {
            // Ensure there is at least one value in the list
            if (valueWeightPairs == null || valueWeightPairs.Count == 0)
            {
                Debug.LogError("No values in the WeightedValue.");
                return default;
            }

            // Create a cumulative weight list
            List<float> cumulativeWeights = new List<float>();
            float totalWeight = 0;

            foreach (var pair in valueWeightPairs)
            {
                totalWeight += pair.weight;
                cumulativeWeights.Add(totalWeight);
            }

            // Generate a random number in the range of the total weight
            float randomWeight = Random.Range(0, totalWeight);

            // Binary search to find the selected value based on the random weight
            int index = cumulativeWeights.BinarySearch(randomWeight);
            if (index < 0)
            {
                // If not found exactly, BinarySearch returns the complement 
                // of the index of the first element that is larger than the value
                index = ~index;
            }

            return valueWeightPairs[index].value;
        }

    }
}