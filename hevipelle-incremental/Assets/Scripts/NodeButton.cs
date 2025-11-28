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
        if (_nodeData.PurchaseCount <= 0)
        {
            if (_nodeData.Dependant1 != null && _nodeData.Dependant1.PurchaseCount < _nodeData.Dependant1Level)
            {
                _button.interactable = false;
                return;
            }
        }

        _button.interactable = true;
        var upgradeCost = _nodeData.GetCurrentCost();
        _indicator.SetActive(_playerData.Faith >= upgradeCost);
    }
}
