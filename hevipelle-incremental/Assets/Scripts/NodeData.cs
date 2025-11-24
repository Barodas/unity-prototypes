using UnityEngine;

[CreateAssetMenu(fileName = "NodeData", menuName = "Scriptable Objects/NodeData")]
public class NodeData : ScriptableObject
{
    // Each node needs a list of dependant nodes for it to be unlocked (not just the node, but the nodes level as well), along with an unlock cost
    // Once unlocked it has a population cap increase (and a small further increase when upgraded)

    public string NodeName;
    public string NodeDescription;
    public int PopulationCapIncrease;
    public int PopulationCapUpgradeIncrease;

    public int UnlockCost;
    public int UpgradeCost;
    public NodeData Dependant1;
    public int Dependant1Level;

    public int PurchaseCount;

    public void Reset()
    {
        PurchaseCount = 0;
    }

    public int GetCurrentCost()
    {
        return PurchaseCount < 1 ? UnlockCost : UnlockCost + (PurchaseCount * UpgradeCost);
    }

    public int GetCurrentPopulationCapIncrease()
    {
        return PurchaseCount < 1 ? 0 : PopulationCapIncrease + (PurchaseCount * PopulationCapUpgradeIncrease);
    }
}   
