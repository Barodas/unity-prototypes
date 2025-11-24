using UnityEngine;
using UnityEngine.UI;

public class NodeButton : MonoBehaviour
{
    [SerializeField] private GameObject _indicator;

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
        _indicator.SetActive(_playerData.Faith >= upgradeCost);
    }
}
