using System;
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
        public LevelTypes levelType;
        public GameObject levelPrefab;
    }

    public float levelTimer => _levelTimer;

    public GameObject GetCarPrefab(CarTypes carType)
    {
        return _carPrefabs.FirstOrDefault(carPrefab => carPrefab.carType == carType)?.carPrefab;
    }

    public GameObject GetLevelPrefab(LevelTypes levelType)
    {
        return _levelPrefabs.FirstOrDefault(levelPrefab => levelPrefab.levelType == levelType)?.levelPrefab;
    }
}
