using System.Collections;
using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameSceneConfig _levelConfig;
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private SceneCollector _sceneCollector;
    [SerializeField] private GameTimerManager _gameTimerManager; // Додаємо GameTimerManager

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
        _sceneCollector.CollectScene(_levelConfig);
        Time.timeScale = 1;

        // Запускаємо таймер тільки якщо цей гравець є MasterClient
        if (PhotonNetwork.IsMasterClient)
        {
            _gameTimerManager.StartTimer(_levelConfig.levelTimer);
        }
    }

    private void FinishGame()
    {
        _gameTimerManager.StopAllCoroutines(); // Якщо потрібно зупинити корутини
        CalculateReward();

        CarEvents.Drift(false);
        Time.timeScale = 0;
    }

    private void UpdatePoints(int amount)
    {
        _points += amount;
    }

    private void CalculateReward()
    {
        ResourcesManager.Instance.ModifyResource(ResourceTypes.TotalPoints, _points);
        ResourcesManager.Instance.ModifyResource(ResourceTypes.Coins, _points);
    }
}