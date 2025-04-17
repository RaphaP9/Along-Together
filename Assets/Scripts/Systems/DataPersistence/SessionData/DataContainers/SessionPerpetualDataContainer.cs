using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionPerpetualDataContainer : MonoBehaviour
{
    public static SessionPerpetualDataContainer Instance { get; private set; }

    public PerpetualData PerpetualData = new();

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
