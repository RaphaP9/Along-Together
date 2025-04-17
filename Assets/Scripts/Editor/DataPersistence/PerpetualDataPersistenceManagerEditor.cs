using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerpetualDataPersistenceManager))]
public class PerpetualDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PerpetualDataPersistenceManager gameDataPersistenceManager = (PerpetualDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            gameDataPersistenceManager.DeleteGameData();
        }
    }
}

