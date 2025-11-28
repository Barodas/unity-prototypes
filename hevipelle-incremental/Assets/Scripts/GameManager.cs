using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;

    [SerializeField] private RectTransform _statsPanel;
    [SerializeField] private RectTransform _nodesPanel;
    [SerializeField] private RectTransform _nodeContentsPanel;
    [SerializeField] private NodeButton _nodeButtonPrefab;
    [SerializeField] private UpgradeButton _upgradeButtonPrefab;
    [SerializeField] private NodeData[] _nodeData;

    private Popup _popup;
    private TextMeshProUGUI _faithText;
    private TextMeshProUGUI _populationText;
    private List<NodeButton> _nodeButtons = new List<NodeButton>();
    private List<GameObject> _nodeContents = new List<GameObject>();
    private List<UpgradeButton> _upgradeButtons = new List<UpgradeButton>();
    private float _tickTimer = 0f;

    public void Initialise(Popup popup)
    {
        _popup = popup;
    }

    public void CreateObjects()
    {
        _faithText = CreateText("FaithText", _statsPanel);
        _populationText = CreateText("PopulationText", _statsPanel);

        for (int i = 0; i < _nodeData.Length; i++)
        {
            NodeData nodeData = _nodeData[i];
            NodeButton nodeButton = CreateNodeButton(nodeData, _nodesPanel);
            _nodeButtons.Add(nodeButton);
        }
    }

    public void Prepare()
    {
        // TODO: Remove once save/load is implemented or to test over multiple sessions
        _playerData.Reset(); 
        foreach (var node in _nodeData)
        {
            node.Reset();
        }

        _playerData.StatsChanged += UpdateStatsText;
        UpdateStatsText();

        foreach (NodeButton button in _nodeButtons)
        {
            _playerData.StatsChanged += button.CheckState;
        }
        _playerData.StatsChanged += UpdateNodeContent;
    }

    public void Begin()
    {
        _popup.SetPopup(
            "Ignite the Crystal!",
            "For a decade our world has been shrouded in darkness. Creatures of shadow have hunted us to near extinction while we search for a way to save our world. We eventually found a method of returning the light. It has taken years to assemble materials for the ritual. \n\nOur preparations are complete. Our faith will be rewarded!\n\nWe must complete the ritual and ignite the crystal. It is the only way to restore light to the world!",
            "The Light will save us!",
            null);
    }

    public void Update()
    {
        if (_nodeData[0].PurchaseCount <= 0)
        {
            // Don't start ticking until first node is purchased.
            return;
        }

        _tickTimer += Time.deltaTime;
        if (_tickTimer < _playerData.TickInterval)
        {
            return;
        }

        _playerData.Tick();
        _tickTimer = 0f;
    }

    private TextMeshProUGUI CreateText(string name, RectTransform parent)
    {
        GameObject textObject = new GameObject(name);
        textObject.transform.SetParent(parent);
        TextMeshProUGUI textComponent = textObject.AddComponent<TextMeshProUGUI>();
        return textComponent;
    }

    private void UpdateStatsText()
    {
        _faithText.text = $"Faith: {Number.Format(_playerData.Faith)}";
        _populationText.text = $"Population: {Number.Format(_playerData.Population)}/{Number.Format(_playerData.PopulationCap)}";
    }

    private NodeButton CreateNodeButton(NodeData data, RectTransform parent)
    {
        NodeButton nodeButton = Instantiate(_nodeButtonPrefab, parent);
        Button button = nodeButton.Initialise(data, _playerData);
        TextMeshProUGUI buttonText = nodeButton.GetComponentInChildren<TextMeshProUGUI>();
        buttonText.text = data.NodeName;
        button.onClick.AddListener(() =>
        {
            InitialiseNodeContent(data);
        });
        return nodeButton;
    }

    private void InitialiseNodeContent(NodeData data)
    {
        foreach (GameObject content in _nodeContents)
        {
            Destroy(content);
        }
        _nodeContents.Clear();
        _upgradeButtons.Clear();

        var header = CreateText("NodeName", _nodeContentsPanel);
        header.text = data.NodeName;
        header.alignment = TextAlignmentOptions.Center;
        header.fontWeight = FontWeight.Bold;
        _nodeContents.Add(header.gameObject);

        var description = CreateText("NodeDescription", _nodeContentsPanel);
        description.text = data.NodeDescription;
        description.enableAutoSizing = true;
        description.fontSizeMax = 30;
        _nodeContents.Add(description.gameObject);

        void PurchaseNode()
        {
            _playerData.Faith -= data.GetCurrentCost();
            data.PurchaseCount++;

            int curPopulationCap = 0;
            foreach (var node in _nodeData)
            {
                curPopulationCap += node.GetCurrentPopulationCapIncrease();
            }
            _playerData.SetPopulationCap(curPopulationCap);

            InitialiseNodeContent(data);
        }

        string purchaseText = data.PurchaseCount <= 0 ? "Unlock" : "Upgrade";
        var buttonText = $"{purchaseText} ({Number.Format(data.GetCurrentCost())} Faith)";
        var upgradeButton = Instantiate(_upgradeButtonPrefab, _nodeContentsPanel);
        var button = upgradeButton.Initialise(data, _playerData);
        button.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        button.onClick.AddListener(() =>
        {
            PurchaseNode();
        });
        _nodeContents.Add(button.gameObject);
        _upgradeButtons.Add(upgradeButton);

        UpdateNodeContent();
    }

    private void UpdateNodeContent()
    {
        foreach (var upgradeButton in _upgradeButtons)
        {
            upgradeButton.CheckState();
        }
    }
}
