using UnityEngine;
using UnityEditor;
using Systems;
using Systems.Core;

[CustomEditor(typeof(MicrowaveGameManager))]
public class MicrowaveGameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 

        MicrowaveGameManager manager = (MicrowaveGameManager)target;

        // Styling for the button
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 14;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.normal.textColor = Color.white;
        buttonStyle.hover.textColor = Color.yellow;
        buttonStyle.padding = new RectOffset(15, 15, 8, 8); // Add some padding for better look

        GUILayout.Space(10); // Add some space before the button

        // Button to start the game
        if (GUILayout.Button("Start Game", buttonStyle, GUILayout.Height(40)))
        {
            manager.StartGame();
        }
        
        // Button to start the game
        if (GUILayout.Button("Stop Game", buttonStyle, GUILayout.Height(40)))
        {
            manager.StopGame();
        }
    }
}