using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectPlace))] // Replace 'YourScript' with the name of your script
public class EditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ObjectPlace script = (ObjectPlace)target;

        if (GUILayout.Button("Setup"))
        {
            // Your code to run when the button is clicked goes here
            script.Setup();
        }
    }
}
