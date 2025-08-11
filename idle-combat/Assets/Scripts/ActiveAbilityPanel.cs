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
            var slot = slotObj.GetComponent<ActiveAbilitySlot>();
            slot.Setup(this, i);
            slots.Add(slot);
        }
    }

    public void AddAbility(AbilityData ability)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (characterData.abilities.Count <= i || characterData.abilities[i] == null)
            {
                if (characterData.abilities.Count <= i)
                    characterData.abilities.Add(ability);
                else
                    characterData.abilities[i] = ability;
                SyncUIWithData();
                break;
            }
        }
    }

    public void RemoveAbility(int slotIndex)
    {
        if (slotIndex < characterData.abilities.Count)
        {
            characterData.abilities[slotIndex] = null;
            SyncUIWithData();
        }
    }

    public void MoveAbility(int fromIndex, int toIndex)
    {
        if (fromIndex < characterData.abilities.Count && toIndex < characterData.abilities.Count && fromIndex != toIndex)
        {
            var temp = characterData.abilities[fromIndex];
            characterData.abilities[fromIndex] = characterData.abilities[toIndex];
            characterData.abilities[toIndex] = temp;
            SyncUIWithData();
        }
    }

    public void SyncUIWithData()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            AbilityData ability = (i < characterData.abilities.Count) ? characterData.abilities[i] : null;
            slots[i].SetAbility(ability);
        }
    }
}