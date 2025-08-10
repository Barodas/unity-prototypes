using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Main")]
    public SizeInt size = new();

    [Header("Visual")]
    public Sprite icon;
    public Color backgroundColor;
}