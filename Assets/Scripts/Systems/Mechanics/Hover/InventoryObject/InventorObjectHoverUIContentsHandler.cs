using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorObjectHoverUIContentsHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InventoryObjectHoverUIHandler inventoryObjectHoverUI;

    [Header("UI Components")]
    [SerializeField] private TextMeshProUGUI objectNameText;
    [SerializeField] private Image objectImage;
    [SerializeField] private TextMeshProUGUI objectClassificationText;
    [SerializeField] private TextMeshProUGUI objectDescriptionText;
    [SerializeField] private TextMeshProUGUI objectSellPriceText;
    [Space]
    [SerializeField] private Transform numericStatsContainer;
    [SerializeField] private Transform numericStatUISample;
    [Space]
    [SerializeField] private List<Image> borders;

    [Header("Settings")]
    [SerializeField] private Color commonColor;
    [SerializeField] private Color uncommonColor;
    [SerializeField] private Color rareColor;
    [SerializeField] private Color epicColor;
    [SerializeField] private Color legendaryColor;

    [Header("Debug")]
    [SerializeField] private bool debug;

    private void OnEnable()
    {
        inventoryObjectHoverUI.OnGenericInventoryObjectIdentifiedSet += InventoryObjectHoverUI_OnGenericInventoryObjectIdentifiedSet;
    }

    private void OnDisable()
    {
        inventoryObjectHoverUI.OnGenericInventoryObjectIdentifiedSet -= InventoryObjectHoverUI_OnGenericInventoryObjectIdentifiedSet;
    }
    public void CompleteSetUI(InventoryObjectSO inventoryObjectSO)
    {
        SetObjectNameText(inventoryObjectSO);
        SetObjectImage(inventoryObjectSO);
        SetObjectClassificationText(inventoryObjectSO);
        SetBordersColor(inventoryObjectSO);
        SetObjectDescriptionText(inventoryObjectSO);
        SetObjectSellPriceText(inventoryObjectSO);
        GenerateNumericStats(inventoryObjectSO);
    }

    private void SetObjectNameText(InventoryObjectSO inventoryObjectSO) => objectNameText.text = inventoryObjectSO._name;
    private void SetObjectImage(InventoryObjectSO inventoryObjectSO) => objectImage.sprite = inventoryObjectSO.sprite;
    private void SetObjectClassificationText(InventoryObjectSO inventoryObjectSO) => objectClassificationText.text = MappingUtilities.MapInventoryObjectRarityType(inventoryObjectSO);
    private void SetObjectDescriptionText(InventoryObjectSO inventoryObjectSO) => objectDescriptionText.text = inventoryObjectSO.description;
    private void SetObjectSellPriceText(InventoryObjectSO inventoryObjectSO) => objectSellPriceText.text = inventoryObjectSO.sellPrice.ToString();

    private void SetBordersColor(InventoryObjectSO inventoryObjectSO)
    {
        Color color;

        switch (inventoryObjectSO.objectRarity)
        {
            case Rarity.Common:
            default:
                color = commonColor;
                break;
            case Rarity.Uncommon:
                color = uncommonColor;
                break;
            case Rarity.Rare:
                color = rareColor;
                break;
            case Rarity.Epic:
                color = epicColor;
                break;
            case Rarity.Legendary:
                color = legendaryColor;
                break;
        }

        UIUtilities.SetImagesColor(borders, color);
    }

    private void GenerateNumericStats(InventoryObjectSO inventoryObjectSO)
    {
        ClearNumericStatsContainer();

        if (inventoryObjectSO.GetNumericEmbeddedStats().Count <= 0)
        {
            numericStatsContainer.gameObject.SetActive(false);
            return;
        }

        foreach (NumericEmbeddedStat numericEmbeddedStat in inventoryObjectSO.GetNumericEmbeddedStats())
        {
            CreateNumericStat(numericEmbeddedStat);
        }
    }

    private void CreateNumericStat(NumericEmbeddedStat numericEmbeddedStat)
    {
        Transform numericStatUI = Instantiate(numericStatUISample, numericStatsContainer);

        InventoryObjectNumericStatUI inventoryObjectNumericStatUI = numericStatUI.GetComponent<InventoryObjectNumericStatUI>();

        if (inventoryObjectNumericStatUI == null)
        {
            if (debug) Debug.Log("Instantiated Numeric Stat UI does not contain a InventoryObjectNumericStatUI component. Set will be ignored.");
            return;
        }

        inventoryObjectNumericStatUI.SetNumericEmbededStat(numericEmbeddedStat);
        numericStatUI.gameObject.SetActive(true);
    }

    private void ClearNumericStatsContainer()
    {
        foreach (Transform child in numericStatsContainer)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void InventoryObjectHoverUI_OnGenericInventoryObjectIdentifiedSet(object sender, InventoryObjectHoverUIHandler.OnGenericInventoryObjectEventArgs e)
    {
        CompleteSetUI(e.genericInventoryObjectIdentified.inventoryObjectSO);
    }
}
