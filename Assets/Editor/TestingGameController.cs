using Components;
using Testing;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(TestingGameController))]
public class TestingGameControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 

        EditorGUILayout.Space();

        TestingGameController controller = (TestingGameController)target;

        EditorGUILayout.LabelField("Heat Up Pool", EditorStyles.boldLabel);
        foreach (SwimmingPool pool in controller.swimmingPoolsManager.SwimmingPools)
        {
            if (GUILayout.Button($"Pool {pool.PoolId}"))
            {
                pool.PoolHeater.HeatUp();
            }
        }
    }
}