using System;
using Cinemachine;
using UnityEngine;

[Serializable]
public class SceneCollector
{
    [SerializeField] private Transform _sceneSpawnPos;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    public GameObject Car {  get; private set;}
    public GameObject Scene { get; private set; }

    public void CollectScene(GameSceneConfig gameSceneConfig)
    {
        var level = SaveManager.PlayerPrefs.LoadEnum(GameSaveKeys.CurrentLevel, LeveTypes.Level1);
        var carType = SaveManager.PlayerPrefs.LoadEnum(GarageSaveKeys.CurrentSelectedCar, CarTypes.CarBasic);

        var scenePrefab = gameSceneConfig.GetLevelPrefab(level);
        var carPrefab = gameSceneConfig.GetCarPrefab(carType);

        if (scenePrefab == null)
        {
            Debug.LogError($"Scene prefab for level {level} not found.");
            return;
        }

        if (carPrefab == null)
        {
            Debug.LogError($"Car prefab for car type {carType} not found.");
            return;
        }

        Scene = InstantiateScene(scenePrefab);

        SpawnCar(Scene, carPrefab);

        ConfigureVirtualCamera();
    }

    private GameObject InstantiateScene(GameObject scenePrefab)
    {
        return UnityEngine.Object.Instantiate(scenePrefab, _sceneSpawnPos.position, _sceneSpawnPos.rotation);
    }

    private void SpawnCar(GameObject scene, GameObject carPrefab)
    {
        var carSpawnPos = scene.GetComponent<TrackManager>()?.GetFreeSpawnPos();
        if (carSpawnPos == null)
        {
            Debug.LogError("No free spawn position found in the scene.");
            return;
        }

        Car = UnityEngine.Object.Instantiate(carPrefab, carSpawnPos.position, carSpawnPos.rotation);
    }

    private void ConfigureVirtualCamera()
    {
        _virtualCamera.LookAt = Car.transform;
        _virtualCamera.Follow = Car.transform;
    }
}
