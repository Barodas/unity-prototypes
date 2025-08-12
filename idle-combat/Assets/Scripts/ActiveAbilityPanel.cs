using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ActiveAbilityPanel : MonoBehaviour
{
    public RectTransform slotContainer; // Assign in inspector: panel across bottom
    public GameObject activeAbilitySlotPrefab; // Prefab for each slot
    public CharacterData characterData; // Assign in inspector

    private List<ActiveAbilitySlot> slots = new List<ActiveAbilitySlot>();

    void Start()
    {
        InitialiseSlots();
        SyncUIWithData();
    }

    void InitialiseSlots()
    {
        slots.Clear();
        for (int i = 0; i < characterData.abilitySlots; i++)
        {
            var slotObj = Instantiate(activeAbilitySlotPrefab, slotContainer);
            var slot = slotObj.GetComponentInChildren<ActiveAbilitySlot>();
            slot.Setup(this, i);
            slots.Add(slot);
        }
    }

    public void AddAbility(AbilityData ability)
    {
        for (int i = 0; i < characterData.abilitySlots; i++)
        {
            if (i >= characterData.abilities.Count || characterData.abilities[i] == null)
            {
                if (i >= characterData.abilities.Count)
                {
                    characterData.abilities.Add(ability);
                }
                else
                {
                    characterData.abilities[i] = ability;
                }
                SyncUIWithData();
                break;
            }
        }
    }

    public void RemoveAbility(int slotIndex)
    {
        if(slotIndex < characterData.abilities.Count)
        {
            // Remove the ability at slotIndex and shift others left
            characterData.abilities.RemoveAt(slotIndex);

            // Ensure the list has abilitySlots elements, pad with nulls if needed
            while (characterData.abilities.Count < characterData.abilitySlots)
            {
                characterData.abilities.Add(null);
            }

            SyncUIWithData();
        }
    }

    public void MoveAbility(int fromIndex, int toIndex)
    {
        if (fromIndex < characterData.abilities.Count && toIndex < characterData.abilitySlots && fromIndex != toIndex)
        {
            var ability = characterData.abilities[fromIndex];
            characterData.abilities.RemoveAt(fromIndex);
            characterData.abilities.Insert(toIndex, ability);

            // Ensure the list is always abilitySlots long
            while (characterData.abilities.Count < characterData.abilitySlots)
            {
                characterData.abilities.Add(null);
            }   

            // If the list is too long (shouldn't happen, but for safety)
            while (characterData.abilities.Count > characterData.abilitySlots)
            {
                characterData.abilities.RemoveAt(characterData.abilities.Count - 1);
            } 

            SyncUIWithData();
        }
    }

    public void SyncUIWithData()
    {
        // Compact the abilities list: move all non-null abilities to the front, nulls to the end
        var compacted = new List<AbilityData>();
        foreach (var ability in characterData.abilities)
        {
            if (ability != null)
                compacted.Add(ability);
        }

        // Pad with nulls to match abilitySlots
        while (compacted.Count < characterData.abilitySlots)
        {
            compacted.Add(null);
        }

        // Overwrite the abilities list with the compacted version
        characterData.abilities = compacted;

        for (int i = 0; i < slots.Count; i++)
        {
            AbilityData ability = (i < characterData.abilities.Count) ? characterData.abilities[i] : null;
            slots[i].SetAbility(ability);
        }
    }
}