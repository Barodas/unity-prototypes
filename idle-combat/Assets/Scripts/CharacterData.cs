using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public int energySlots = 5;
    public int abilitySlots = 3;
    public int maxHealth = 100;
    public int curHealth = 100;

    public List<AbilityData> abilities = new List<AbilityData>();
}
