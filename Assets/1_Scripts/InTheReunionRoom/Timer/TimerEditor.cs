using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Timer))]
public class TimerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Timer myScript = (Timer)target;
        if(GUILayout.Button("Start Timer"))
        {
            myScript.StartTimer();
        }
        if(GUILayout.Button("Reset Timer"))
        {
            myScript.ResetTimer();
        }
    }
}