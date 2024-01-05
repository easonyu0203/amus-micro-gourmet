using UnityEngine;
using UnityEditor;
using Components; 

[CustomEditor(typeof(PoolHeater))]
public class PoolHeaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Draws the default inspector

        EditorGUILayout.Space(); // Add some spacing for better layout

        // Draw a header
        EditorGUILayout.LabelField("Pool Heater Controls", EditorStyles.boldLabel);

        PoolHeater poolHeater = (PoolHeater)target;

        // Add a button to the inspector
        if (GUILayout.Button("Heat Up"))
        {
            poolHeater.HeatUp(); // Call the HeatUp method when the button is pressed
        }
    }
}