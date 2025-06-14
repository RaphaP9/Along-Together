using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCollisionHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [Header("Settings")]
    [SerializeField] private LayerMask terrainLayerMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GeneralUtilities.CheckGameObjectInLayerMask(collision.gameObject, terrainLayerMask)) return;

        _rigidbody2D.velocity = Vector2.zero;
    }
}
