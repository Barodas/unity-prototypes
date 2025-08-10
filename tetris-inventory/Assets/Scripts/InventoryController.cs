using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class InventoryController : MonoBehaviour
{
    public Inventory inventory { get; private set; }

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Check if mouse is inside a any grid.
            if (!inventory.ReachedBoundary(inventory.GetSlotAtMouseCoords(), inventory.gridOnMouse))
            {
                if (inventory.selectedItem)
                {
                    Item oldSelectedItem = inventory.selectedItem;
                    Item overlapItem = inventory.GetItemAtMouseCoords();

                    if (overlapItem != null)
                    {
                        inventory.SwapItem(overlapItem, oldSelectedItem);
                    }
                    else
                    {
                        inventory.MoveItem(oldSelectedItem);
                    }
                }
                else
                {
                    SelectItemWithMouse();
                }
            }
        }

        // Remove an item from the inventory
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RemoveItemWithMouse();
        }

        // Generates a random item in the inventory
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.AddItem(inventory.itemsData[UnityEngine.Random.Range(0, inventory.itemsData.Length)]);
        }

        if (inventory.selectedItem != null)
        {
            MoveSelectedItemToMouse();

            if (Input.GetKeyDown(KeyCode.R))
            {
                inventory.selectedItem.Rotate();
            }
        }
    }

    private void SelectItemWithMouse()
    {
        Item item = inventory.GetItemAtMouseCoords();

        if (item != null)
        {
            inventory.SelectItem(item);
        }
    }

    private void RemoveItemWithMouse()
    {
        Item item = inventory.GetItemAtMouseCoords();

        if (item != null)
        {
            inventory.RemoveItem(item);
        }
    }

    private void MoveSelectedItemToMouse()
    {
        inventory.selectedItem.rectTransform.position = new Vector3(
                Input.mousePosition.x
                    + ((inventory.selectedItem.correctedSize.width * InventorySettings.slotSize.x) / 2)
                    - InventorySettings.slotSize.x / 2,
                Input.mousePosition.y
                    - ((inventory.selectedItem.correctedSize.height * InventorySettings.slotSize.y) / 2)
                    + InventorySettings.slotSize.y / 2,
                Input.mousePosition.z
            );
    }
}