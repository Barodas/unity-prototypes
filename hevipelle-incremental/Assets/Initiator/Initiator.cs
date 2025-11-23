using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class Initiator : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LoadingScreen _loadingScreen;
    [SerializeField] private EventSystem _eventSystem;

    [SerializeField] private GameManager _gameManager;

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
    }

    private async UniTask InitialiseObjects()
    {
        // Initialise services and game systems (such as analytics or input)
    }

    private async UniTask CreateObjects()
    {
        // Instantiate game objects 
    }

    private void PrepareGame()
    {
        // Setup up previously instantiated objects (positions, states, etc)
    }

    private async UniTask BeginGame()
    {
        // Start sequence of the game (fade in elements, zoom camera, etc)
    }
}
