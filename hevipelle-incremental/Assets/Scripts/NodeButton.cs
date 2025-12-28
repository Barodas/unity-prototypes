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
            if (_nodeData.Dependant1 != null)
            {
                // Hide the button if the dependant hasnt been purchased/unlocked
                if (_nodeData.Dependant1.PurchaseCount <= 0)
                {
                    _button.interactable = false;
                    _button.gameObject.SetActive(false);
                    _indicator.SetActive(false);
                    return;
                }
                // Show the button, but disable until the dependant has been ugraded enough
                else if (_nodeData.Dependant1.PurchaseCount < _nodeData.Dependant1Level)
                {
                    _button.gameObject.SetActive(true);
                    _button.interactable = false;
                    _indicator.SetActive(false);
                    return;
                }
            }
        }

        _button.interactable = true;
        var upgradeCost = _nodeData.GetCurrentCost();
        _indicator.SetActive(_playerData.Faith >= upgradeCost);
    }
}
