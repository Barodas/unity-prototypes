using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private RectTransform _panel;
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _contents;
    [SerializeField] private Button _button1;
    [SerializeField] private Button _button2;

    private TextMeshProUGUI _button1Text;
    private TextMeshProUGUI _button2Text;

    public void Initialise()
    {
        _button1Text = _button1.GetComponentInChildren<TextMeshProUGUI>();
        _button2Text = _button2.GetComponentInChildren<TextMeshProUGUI>();
        Hide();
    }

    public void SetPopup(string header, string contents, string button1Text, string button2Text)
    {
        _header.text = header;
        _contents.text = contents;

        _button1Text.text = button1Text;
        _button1.onClick.RemoveAllListeners();
        _button1.onClick.AddListener(Hide);

        if (string.IsNullOrEmpty(button2Text))
        {
            _button2.gameObject.SetActive(false);
        }
        else
        {
            _button2.gameObject.SetActive(true);
            _button2Text.text = button2Text;
            _button2.onClick.RemoveAllListeners();
            _button2.onClick.AddListener(Hide);
        }

        Show();
    }

    public void Show()
    {
        _panel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _panel.gameObject.SetActive(false);
    }
}
