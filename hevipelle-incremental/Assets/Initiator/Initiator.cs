using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Initiator : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private EventSystem _eventSystem;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Popup _popup;

    private async void Start()
    {
        BindObjects();
        _loadingScreen.Show();
        await InitialiseObjects();
        await CreateObjects();
        PrepareGame();
        _loadingScreen.Hide();
        await BeginGame();
    }

    private void BindObjects()
    {
        _mainCamera = Instantiate(_mainCamera);
        _loadingScreen = Instantiate(_loadingScreen);
        _eventSystem = Instantiate(_eventSystem);
        _gameManager = Instantiate(_gameManager);
        _popup = Instantiate(_popup, _gameManager.transform);
    }

    private async UniTask InitialiseObjects()
    {
        _popup.Initialise();
        _gameManager.Initialise(_popup);
    }

    private async UniTask CreateObjects()
    {
        _gameManager.CreateObjects();
    }

    private void PrepareGame()
    {
        _gameManager.Prepare();
    }

    private async UniTask BeginGame()
    {
        _gameManager.Begin();
    }
}
