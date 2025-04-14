using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameDataPersistenceManager))]
public class GameDataPersistenceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameDataPersistenceManager gameDataPersistenceManager = (GameDataPersistenceManager)target;

        if (GUILayout.Button("Delete Data File"))
        {
            gameDataPersistenceManager.DeleteGameData();
        }
    }
}

