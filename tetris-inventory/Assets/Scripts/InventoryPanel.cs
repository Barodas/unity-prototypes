using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject itemPrefab;
    public InventoryItemData testItemData1x1;
    public InventoryItemData testItemData2x2;
    public InventoryItemData testItemData3x1;
    public int columns = 10;
    public int rows = 6;
    
    private List<RectTransform> slotRects = new List<RectTransform>();
    
    void Start()
    {
        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject slotGO = Instantiate(slotPrefab, transform);
                slotRects.Add(slotGO.GetComponent<RectTransform>());
            }
        }
    }

    public void AddTestItem(InventoryItemData testItemData)
    {
        GameObject itemGO = Instantiate(itemPrefab, transform);
        var itemUI = itemGO.GetComponent<InventoryItemUI>();
        itemUI.SetItem(testItemData);

        // Place item over the first slot (top-left)
        if (slotRects.Count > 0)
        {
            itemGO.GetComponent<RectTransform>().anchoredPosition = slotRects[0].anchoredPosition;
        }

        itemGO.transform.SetAsLastSibling();
    }

    public void AddTestItem1x1()
    {
        AddTestItem(testItemData1x1);
    }

    public void AddTestItem2x2()
    {
        AddTestItem(testItemData2x2);
    }
    
    public void AddTestItem3x1()
    {
        AddTestItem(testItemData3x1);
    }
}