using System.Collections;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameSceneConfig _levelConfig;
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private SceneCollector _sceneCollector;

    private DriftManager _driftManager = new DriftManager();

    private Coroutine _levelTimer;
    private int _points;

    private const float _second = 1f;

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
        GameEvents.OnPointsUpdate += UpdatePoints;

        _driftManager.Subscribe();
    }

    private void UnSubscribe()
    {
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
        _levelTimer = StartCoroutine(GameTimer(_levelConfig.levelTimer));
    }

    private void FinishGame()
    {
        if (_levelTimer != null)
        {
            StopCoroutine(_levelTimer);
        }
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

    private IEnumerator GameTimer(float _time)
    {
        float time = _time;
        while (time > 0)
        {
            GameEvents.TimerUpdate(time);
            yield return new WaitForSeconds(_second);
            time -= _second;
        }
        FinishGame();
    }
}