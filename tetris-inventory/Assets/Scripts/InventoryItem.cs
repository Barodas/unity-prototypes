using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    public Image iconImage;
    public InventoryItemData itemData;

    public void SetItem(InventoryItemData data)
    {
        itemData = data;
        iconImage.sprite = data.icon;
        switch(itemData.shape)
        {
            case ItemShape.Single:
                iconImage.rectTransform.sizeDelta = new Vector2(60, 60);
                break;
            case ItemShape.Line2:
                iconImage.rectTransform.sizeDelta = new Vector2(60, 120);
                break;
            case ItemShape.Line3:
                iconImage.rectTransform.sizeDelta = new Vector2(60, 180);
                break;
            case ItemShape.Square2x2:
                iconImage.rectTransform.sizeDelta = new Vector2(120, 120);
                break;
            case ItemShape.LShape:
                iconImage.rectTransform.sizeDelta = new Vector2(180, 120);
                break;
            case ItemShape.Custom:
                // Handle custom shapes if needed
                break;
        }
    }
}