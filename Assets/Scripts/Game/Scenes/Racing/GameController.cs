using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private ResourcesManager _resourcesManager;

    private DriftManager _driftManager = new DriftManager();

    private float points;

    private void Awake()
    {
        Init();
        Subscribe();
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

    private void UpdatePoints(float amount)
    {
        points += amount;
    }
}