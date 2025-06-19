using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTooltipHoverUIHandler : MonoBehaviour
{
    private void OnEnable()
    {
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEnter += ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverEnter;
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverExit += ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverExit;

        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEnter += TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverEnter;
        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverExit += TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverExit;
    }

    private void OnDisable()
    {
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEnter -= ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverEnter;
        ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverExit -= ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverExit;

        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEnter -= TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverEnter;
        TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverExit -= TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverExit;
    }

    #region Object Subscriptions
    private void ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverEnter(object sender, ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEventArgs e)
    {
        
    }
    private void ObjectShopInventoryHoverHandler_OnObjectShopInventoryHoverExit(object sender, ObjectShopInventoryHoverHandler.OnObjectShopInventoryHoverEventArgs e)
    {
        
    }
    #endregion

    #region Treat Subscriptions
    private void TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverEnter(object sender, TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEventArgs e)
    {
        
    }

    private void TreatShopInventoryHoverHandler_OnTreatShopInventoryHoverExit(object sender, TreatShopInventoryHoverHandler.OnTreatShopInventoryHoverEventArgs e)
    {
        
    }
    #endregion
}
