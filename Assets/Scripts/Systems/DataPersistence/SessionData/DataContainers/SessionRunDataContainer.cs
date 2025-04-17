using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionRunDataContainer : MonoBehaviour
{
    public static SessionRunDataContainer Instance { get; private set; }

    public RunData RunData = new();

    private void Awake()
    {
        SetSingleton();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
