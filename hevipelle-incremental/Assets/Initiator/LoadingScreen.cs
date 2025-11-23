using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _loadingText;

    private string _baseText = "Loading";
    private float _dotDelay = 0.5f;

    private float _dotTimer = 0f;
    private int _dotCount = 0;

    private void Awake()
    {
        Hide();
    }

    void Update()
    {
        _dotTimer += Time.deltaTime;
        if (_dotTimer >= _dotDelay)
        {
            _dotCount = (_dotCount + 1) % 4;
            _loadingText.text = _baseText + new string('.', _dotCount);
            _dotTimer = 0f;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
