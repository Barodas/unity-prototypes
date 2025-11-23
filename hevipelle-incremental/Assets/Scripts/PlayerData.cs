using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public int Faith;
    public int Population;
    public int PopulationCap;
    public int Divinity;

    public void Clear()
    {
        Faith = 0;
        Population = 0;
        PopulationCap = 0;
        Divinity = 0;
    }

    public void CalcFaith()
    {
        // TODO: Determine how Divinity affects faith gain
        Faith += Population;
    }
}
