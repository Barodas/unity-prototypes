using UnityEngine;

public enum EnergyType
{
    Martial = 0,
    Fire = 1,
    Water = 2,
    Earth = 3,
    Air = 4
}

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{
    public EnergyType energyType = EnergyType.Martial;
    public int energyCost = 1;

    public EnergyType generateType = EnergyType.Martial;
    public int generateAmount = 0;

    public int damage = 10;

    public string description = "";
    public Sprite icon;
}
