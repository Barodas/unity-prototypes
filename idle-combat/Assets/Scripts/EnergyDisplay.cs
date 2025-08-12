using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

public class EnergyDisplay : MonoBehaviour
{
    public CharacterData characterData;
    public GameObject energySlotPrefab;
    public float orbSpacing = 0.5f;

    List<SpriteRenderer> orbSprites = new List<SpriteRenderer>();

    // Define colors for each EnergyType
    private static readonly Dictionary<EnergyType, Color> energyColors = new Dictionary<EnergyType, Color>
    {
        { EnergyType.Martial, Color.gray },
        { EnergyType.Fire, Color.red },
        { EnergyType.Water, Color.blue },
        { EnergyType.Earth, new Color(0.5f, 0.25f, 0f) },
        { EnergyType.Air, Color.cyan }
    };

    void Start()
    {
        for(int i = 0; i < characterData.energySlots; i++)
        {
            GameObject energySlot = Instantiate(energySlotPrefab, transform);
            SpriteRenderer spriteRenderer = energySlot.GetComponent<SpriteRenderer>();
            orbSprites.Add(spriteRenderer);
            energySlot.transform.localPosition = new Vector3(i * orbSpacing, 0, 0);
        }
        UpdateEnergyDisplay();
    }

    public void UpdateEnergyDisplay()
    {
        for (int i = 0; i < characterData.energySlots; i++)
        {
            if (i < characterData.energy.Count)
            {
                EnergyType energyType = characterData.energy[i];
                orbSprites[i].color = energyColors[energyType];
            }
            else
            {
                orbSprites[i].color = Color.white;
            }
        }
    }

}
