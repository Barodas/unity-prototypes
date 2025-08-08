using UnityEngine;

public enum ItemShape
{
    Single,      // 1x1
    Line2,       // 1x2
    Line3,       // 1x3
    Square2x2,   // 2x2
    LShape,      // L shape
    Custom       // For future custom shapes
}

[CreateAssetMenu(fileName = "InventoryItemData", menuName = "Inventory/Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemShape shape;

    // Example: shape definition as occupied grid positions
    public Vector2Int[] occupiedSlots;
}
