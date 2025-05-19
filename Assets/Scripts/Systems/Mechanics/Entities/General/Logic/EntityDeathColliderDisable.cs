using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDeathColliderDisable : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private EntityHealth entityHealth;
    [SerializeField] private Collider2D _collider2D;

    private void OnEnable()
    {
        entityHealth.OnEntityDeath += EntityHealth_OnEntityDeath;
    }

    private void OnDisable()
    {
        entityHealth.OnEntityDeath -= EntityHealth_OnEntityDeath;
    }

    private void DisableCollider()
    {
        _collider2D.enabled = false;
    }

    private void EntityHealth_OnEntityDeath(object sender, System.EventArgs e)
    {
        DisableCollider();
    }
}
