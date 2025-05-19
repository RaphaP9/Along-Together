using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSortingOrderRenderingHandler : SortingOrderRenderingHandler
{
    [Header("Canvas Components")]
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private bool respectListingOrder;

    protected override void UpdateSortingOrder(int sortingOrder)
    {
        int aditionalOrder = 0;

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.sortingOrder = respectListingOrder ? sortingOrder + aditionalOrder : sortingOrder;
            aditionalOrder++;
        }
    }
}
