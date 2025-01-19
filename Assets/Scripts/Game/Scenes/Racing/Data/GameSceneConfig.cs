using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameScene", menuName = "ScriptableObjects/Game/RacingScene", order = 1)]
public class GameSceneConfig : ScriptableObject
{
    [SerializeField] private float _levelTimer;
    [SerializeField] private CarPrefab[] _carPrefabs;
    [SerializeField] private LevelPrefabs[] _levelPrefabs;

    [Serializable]
    public class CarPrefab
    {
        public CarTypes carType;
        public GameObject carPrefab;
    }
    [Serializable]
    public class LevelPrefabs
    {
        public LeveTypes levelType;
        public GameObject levelPrefab;
    }

    public float levelTimer => _levelTimer;

    public GameObject GetCarPrefab(CarTypes carType)
    {
        return _carPrefabs.FirstOrDefault(carPrefab => carPrefab.carType == carType)?.carPrefab;
    }
    public GameObject GetLevelPrefab(LeveTypes levelType)
    {
        return _levelPrefabs.FirstOrDefault(levelPrefab => levelPrefab.levelType == levelType)?.levelPrefab;
    }
}
