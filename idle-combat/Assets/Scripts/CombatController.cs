using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CombatController : MonoBehaviour
{
    public CharacterData characterData;
    public DummyTarget dummyTarget;

    public EnergyDisplay energyDisplay; // TODO: Might be better to create an overall character class that contains reference to this and data

    void Start()
    {
        StartCoroutine(CombatLoop());
    }

    IEnumerator CombatLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            TryUseAbility();
            energyDisplay.UpdateEnergyDisplay();
        }
    }

    void TryUseAbility()
    {
        foreach (var ability in characterData.abilities)
        {
            if (ability == null) continue;

            // Count available energy of the required type
            int available = 0;
            foreach (var e in characterData.energy)
            {
                if (e == ability.energyType) available++;
            }

            if (available >= ability.energyCost)
            {
                // Spend energy
                int spent = 0;
                for (int i = characterData.energy.Count - 1; i >= 0 && spent < ability.energyCost; i--)
                {
                    if (characterData.energy[i] == ability.energyType)
                    {
                        characterData.energy.RemoveAt(i);
                        spent++;
                    }
                }

                // Generate energy, but do not exceed energySlots
                int currentTotal = characterData.energy.Count;
                int slotsLeft = characterData.energySlots - currentTotal;
                int energyToGenerate = Mathf.Min(ability.generateAmount, slotsLeft);
                for (int i = 0; i < energyToGenerate; i++)
                {
                    characterData.energy.Add(ability.generateType);
                }

                // Deal damage
                dummyTarget.TakeDamage(ability.damage);

                break; // Only use one ability per tick
            }
        }
    }
}