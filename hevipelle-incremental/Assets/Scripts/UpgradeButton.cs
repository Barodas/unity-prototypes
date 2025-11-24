using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    private NodeData _nodeData;
    private PlayerData _playerData;

    private Button _button;

    public Button Initialise(NodeData data, PlayerData playerData)
    {
        _nodeData = data;
        _playerData = playerData;
        _button = GetComponent<Button>();
        return _button;
    }

    public void CheckState()
    {
        var upgradeCost = _nodeData.GetCurrentCost();
        _button.interactable = _playerData.Faith >= upgradeCost;
    }
}
