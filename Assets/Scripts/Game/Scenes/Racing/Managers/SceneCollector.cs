using System;
using Cinemachine;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class SceneCollector
{
    
    [SerializeField] private Transform _sceneSpawnPos;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    public GameObject Car { get; private set; }
    public GameObject Scene { get; private set; }

    public void CollectScene(GameSceneConfig gameSceneConfig, InputSystem inputSystem)
    {
        var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (!TryGetRoomProperties(roomProperties, out var level, out var carType))
            return;

        if (!TryGetPrefabs(gameSceneConfig, level, carType, out var scenePrefab, out var carPrefab))
            return;

        Scene = InstantiateScene(scenePrefab);
        SpawnCar(Scene, carPrefab, inputSystem);

        ConfigureVirtualCamera();
    }

    private bool TryGetRoomProperties(Hashtable roomProperties, out LevelTypes level, out CarTypes carType)
    {
        level = default;
        carType = default;

        if (!roomProperties.TryGetValue("Level", out object levelObj) ||
            !roomProperties.TryGetValue("CarType", out object carTypeObj))
        {
            Debug.LogError("Failed to retrieve level or car type from room properties.");
            return false;
        }

        level = (LevelTypes)levelObj;
        carType = (CarTypes)carTypeObj;
        return true;
    }

    private bool TryGetPrefabs(GameSceneConfig gameSceneConfig, LevelTypes level, CarTypes carType,
                           out GameObject scenePrefab, out GameObject carPrefab)
    {
        scenePrefab = gameSceneConfig.GetLevelPrefab(level);
        carPrefab = gameSceneConfig.GetCarPrefab(carType);

        if (scenePrefab == null)
        {
            Debug.LogError($"Scene prefab for level {level} not found.");
            return false;
        }

        if (carPrefab == null)
        {
            Debug.LogError($"Car prefab for car type {carType} not found.");
            return false;
        }

        return true;
    }

    private GameObject InstantiateScene(GameObject scenePrefab)
    {
        return UnityEngine.Object.Instantiate(scenePrefab, _sceneSpawnPos.position, _sceneSpawnPos.rotation);
    }

    private void SpawnCar(GameObject scene, GameObject carPrefab, InputSystem inputSystem)
    {
        Transform[] allChildren = scene.GetComponentsInChildren<Transform>();

        Transform carSpawnPos = null;
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("SpawnPos"))
            {
                carSpawnPos = child;
                break;
            }
        }
        if (carSpawnPos == null)
        {
            Debug.LogError("No free spawn position found in the scene.");
            return;
        }
        Car = PhotonNetwork.Instantiate(carPrefab.name, carSpawnPos.position, carSpawnPos.rotation);
        Car.GetComponent<CarController>().Init(inputSystem);
        UnityEngine.Object.Destroy(carSpawnPos.gameObject);
    }

    private void ConfigureVirtualCamera()
    {
        if (Car == null) return;
        _virtualCamera.LookAt = Car.transform;
        _virtualCamera.Follow = Car.transform;
    }
}