using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameSceneConfig _levelConfig;
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private SceneCollector _sceneCollector;
    [SerializeField] private GameTimerManager _gameTimerManager; 
    [SerializeField] private InputSystem _inputSystem;

    private DriftManager _driftManager = new DriftManager();

    private int _points;

    private void Awake()
    {
        Init();
        Subscribe();
    }

    private void Start()
    {
        StartGame();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        GameEvents.OnGameFinish += FinishGame;
        GameEvents.OnPointsUpdate += UpdatePoints;

        _driftManager.Subscribe();
    }

    private void UnSubscribe()
    {
        GameEvents.OnGameFinish -= FinishGame;
        GameEvents.OnPointsUpdate -= UpdatePoints;

        _driftManager.UnSubscribe();
    }

    private void Init()
    {
        _resourcesManager.Init();
    }

    private void StartGame()
    {
        _sceneCollector.CollectScene(_levelConfig, _inputSystem);
        Time.timeScale = 1;

        if (PhotonNetwork.IsMasterClient)
        {
            _gameTimerManager.StartTimer(_levelConfig.levelTimer);
        }
    }

    private void FinishGame()
    {
        _gameTimerManager.StopAllCoroutines();    
        
        UIManager.Instance.ShowPopup(PopupTypes.WinGame);

        WinPopup winPopup = (WinPopup) UIManager.Instance.GetPopup(PopupTypes.WinGame);
        int points = _points + _driftManager.Score;
        winPopup.Init(points);

        CarEvents.Drift(false);
        Time.timeScale = 0;
    }

    private void UpdatePoints(int amount)
    {
        _points += amount;
    }
}