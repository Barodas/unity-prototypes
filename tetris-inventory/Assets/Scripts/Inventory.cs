using UnityEngine;

public static class InventorySettings
{
    public static readonly Vector2Int slotSize = new(60, 60);
    public static readonly float slotScale = 1f;
    public static readonly float rotationAnimationSpeed = 30f;
}

public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    public ItemData[] itemsData;
    public Item itemPrefab;
    public InventoryGrid gridOnMouse { get; set; } // TODO: rename to under 
    public InventoryGrid[] grids { get; private set; }
    public Item selectedItem { get; private set; }
    
    private void Awake()
    {
        grids = GameObject.FindObjectsByType<InventoryGrid>(FindObjectsSortMode.None);
    }

    public void SelectItem(Item item)
    {
        ClearItemReferences(item);
        selectedItem = item;
        selectedItem.rectTransform.SetParent(transform);
        selectedItem.rectTransform.SetAsLastSibling();
    }

    private void DeselectItem()
    {
        selectedItem = null;
    }

    public void AddItem(ItemData itemData)
    {
        for (int g = 0; g < grids.Length; g++)
        {
            for (int y = 0; y < grids[g].gridSize.y; y++)
            {
                for (int x = 0; x < grids[g].gridSize.x; x++)
                {
                    Vector2Int slotPosition = new Vector2Int(x, y);

                    for (int r = 0; r < 2; r++)
                    {
                        if (r == 0)
                        {
                            if (!ExistsItem(slotPosition, grids[g], itemData.size.width, itemData.size.height))
                            {
                                Item newItem = Instantiate(itemPrefab);
                                newItem.rectTransform = newItem.GetComponent<RectTransform>();
                                newItem.rectTransform.SetParent(grids[g].rectTransform);
                                newItem.rectTransform.sizeDelta = new Vector2(
                                    itemData.size.width * InventorySettings.slotSize.x,
                                    itemData.size.height * InventorySettings.slotSize.y
                                );

                                newItem.indexPosition = slotPosition;
                                newItem.inventory = this;

                                for (int xx = 0; xx < itemData.size.width; xx++)
                                {
                                    for (int yy = 0; yy < itemData.size.height; yy++)
                                    {
                                        int slotX = slotPosition.x + xx;
                                        int slotY = slotPosition.y + yy;

                                        grids[g].items[slotX, slotY] = newItem;
                                        grids[g].items[slotX, slotY].data = itemData;
                                    }
                                }

                                newItem.rectTransform.localPosition = IndexToInventoryPosition(newItem);
                                newItem.inventoryGrid = grids[g];
                                return;
                            }
                        }

                        if (r == 1)
                        {
                            if (!ExistsItem(slotPosition, grids[g], itemData.size.height, itemData.size.width))
                            {
                                Item newItem = Instantiate(itemPrefab);
                                newItem.Rotate();

                                newItem.rectTransform = newItem.GetComponent<RectTransform>();
                                newItem.rectTransform.SetParent(grids[g].rectTransform);
                                newItem.rectTransform.sizeDelta = new Vector2(
                                    itemData.size.width * InventorySettings.slotSize.x,
                                    itemData.size.height * InventorySettings.slotSize.y
                                );

                                newItem.indexPosition = slotPosition;
                                newItem.inventory = this;

                                for (int xx = 0; xx < itemData.size.height; xx++)
                                {
                                    for (int yy = 0; yy < itemData.size.width; yy++)
                                    {
                                        int slotX = slotPosition.x + xx;
                                        int slotY = slotPosition.y + yy;

                                        grids[g].items[slotX, slotY] = newItem;
                                        grids[g].items[slotX, slotY].data = itemData;
                                    }
                                }

                                newItem.rectTransform.localPosition = IndexToInventoryPosition(newItem);
                                newItem.inventoryGrid = grids[g];

                                return;
                            }
                        }
                    }
                }
            }
        }

        Debug.Log("(Inventory) Not enough slots found to add the item!");
    }

    public void RemoveItem(Item item)
    {
        if (item != null)
        {
            ClearItemReferences(item);
            Destroy(item.gameObject);
        }
    }

    public void MoveItem(Item item, bool deselectItemInEnd = true)
    {
        Vector2Int slotPosition = GetSlotAtMouseCoords();

        if (ReachedBoundary(slotPosition, gridOnMouse, item.correctedSize.width, item.correctedSize.height))
        {
            Debug.Log("Bounds");
            return;
        }

        if (ExistsItem(slotPosition, gridOnMouse, item.correctedSize.width, item.correctedSize.height))
        {
            Debug.Log("Item");
            return;
        }

        item.indexPosition = slotPosition;
        item.rectTransform.SetParent(gridOnMouse.rectTransform);

        for (int x = 0; x < item.correctedSize.width; x++)
        {
            for (int y = 0; y < item.correctedSize.height; y++)
            {
                int slotX = item.indexPosition.x + x;
                int slotY = item.indexPosition.y + y;

                gridOnMouse.items[slotX, slotY] = item;
            }
        }

        item.rectTransform.localPosition = IndexToInventoryPosition(item);
        item.inventoryGrid = gridOnMouse;

        if (deselectItemInEnd)
        {
            DeselectItem();
        }
    }

    public void SwapItem(Item overlapItem, Item oldSelectedItem)
    {
        if (ReachedBoundary(overlapItem.indexPosition, gridOnMouse, oldSelectedItem.correctedSize.width, oldSelectedItem.correctedSize.height))
        {
            return;
        }

        ClearItemReferences(overlapItem);

        if (ExistsItem(overlapItem.indexPosition, gridOnMouse, oldSelectedItem.correctedSize.width, oldSelectedItem.correctedSize.height))
        {
            RevertItemReferences(overlapItem);
            return;
        }

        SelectItem(overlapItem);
        MoveItem(oldSelectedItem, false);
    }

    public void ClearItemReferences(Item item)
    {
        for (int x = 0; x < item.correctedSize.width; x++)
        {
            for (int y = 0; y < item.correctedSize.height; y++)
            {
                int slotX = item.indexPosition.x + x;
                int slotY = item.indexPosition.y + y;

                item.inventoryGrid.items[slotX, slotY] = null;
            }
        }
    }

    public void RevertItemReferences(Item item)
    {
        for (int x = 0; x < item.correctedSize.width; x++)
        {
            for (int y = 0; y < item.correctedSize.height; y++)
            {
                int slotX = item.indexPosition.x + x;
                int slotY = item.indexPosition.y + y;

                item.inventoryGrid.items[slotX, slotY] = item;
            }
        }
    }

    public bool ExistsItem(Vector2Int slotPosition, InventoryGrid grid, int width = 1, int height = 1)
    {
        if (ReachedBoundary(slotPosition, grid, width, height))
        {
            Debug.Log("Bounds2");
            return true;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int slotX = slotPosition.x + x;
                int slotY = slotPosition.y + y;

                if (grid.items[slotX, slotY] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool ReachedBoundary(Vector2Int slotPosition, InventoryGrid gridReference, int width = 1, int height = 1)
    {
        if (slotPosition.x + width > gridReference.gridSize.x || slotPosition.x < 0)
        {
            return true;
        }

        if (slotPosition.y + height > gridReference.gridSize.y || slotPosition.y < 0)
        {
            return true;
        }

        return false;
    }

    public Vector3 IndexToInventoryPosition(Item item)
    {
        Vector3 inventorizedPosition =
            new()
            {
                x = item.indexPosition.x * InventorySettings.slotSize.x
                    + InventorySettings.slotSize.x * item.correctedSize.width / 2,
                y = -(item.indexPosition.y * InventorySettings.slotSize.y
                    + InventorySettings.slotSize.y * item.correctedSize.height / 2
                )
            };

        return inventorizedPosition;
    }

    public Vector2Int GetSlotAtMouseCoords()
    {
        if (gridOnMouse == null)
        {
            return Vector2Int.zero;
        }

        Vector2 gridPosition =
            new(
                Input.mousePosition.x - gridOnMouse.rectTransform.position.x,
                gridOnMouse.rectTransform.position.y - Input.mousePosition.y
            );

        Vector2Int slotPosition =
            new(
                (int)(gridPosition.x / (InventorySettings.slotSize.x * InventorySettings.slotScale)),
                (int)(gridPosition.y / (InventorySettings.slotSize.y * InventorySettings.slotScale))
            );

        return slotPosition;
    }

    public Item GetItemAtMouseCoords()
    {
        Vector2Int slotPosition = GetSlotAtMouseCoords();

        if (!ReachedBoundary(slotPosition, gridOnMouse))
        {
            return GetItemFromSlotPosition(slotPosition);
        }

        return null;
    }

    public Item GetItemFromSlotPosition(Vector2Int slotPosition)
    {
        return gridOnMouse.items[slotPosition.x, slotPosition.y];
    }
}