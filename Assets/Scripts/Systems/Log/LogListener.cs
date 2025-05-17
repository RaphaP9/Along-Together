using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogListener : MonoBehaviour
{
    private void OnEnable()
    {
        EntityExplosion.OnAnyEntityExplosion += EntityExplosion_OnAnyEntityExplosion;
    }

    private void OnDisable()
    {
        EntityExplosion.OnAnyEntityExplosion -= EntityExplosion_OnAnyEntityExplosion;
    }


    #region Subscriptions
    private void EntityExplosion_OnAnyEntityExplosion(object sender, EntityExplosion.OnEntityExplosionEventArgs e)
    {
        GameLogManager.Instance.Log("Entity/Explosion");
    }
    #endregion
}
