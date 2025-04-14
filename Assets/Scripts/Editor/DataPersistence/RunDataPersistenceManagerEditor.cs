using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RunDataPersistenceManager))]
public class RunDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RunDataPersistenceManager runDataPersistenceManager = (RunDataPersistenceManager)target;

        if(GUILayout.Button("Delete Data File"))
        {
            runDataPersistenceManager.DeleteGameData();
        }
    }
}
