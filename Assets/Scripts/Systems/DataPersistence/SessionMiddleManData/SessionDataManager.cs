using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionDataManager : MonoBehaviour
{
    public static SessionDataManager Instance { get; private set; }

    public PerpetualData PerpetualData = new();
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
