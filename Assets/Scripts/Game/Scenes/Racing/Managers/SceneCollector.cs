using System;
using System.Linq;
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
        if (!TryGetRoomProperties(out var level, out var carType))
            return;

        if (!TryGetPrefabs(gameSceneConfig, level, carType, out var scenePrefab, out var carPrefab))
            return;

        Scene = InstantiateScene(scenePrefab);
        SpawnCar(Scene, carPrefab, inputSystem);

        ConfigureVirtualCamera();
    }

    private bool TryGetRoomProperties(out LevelTypes level, out CarTypes carType)
    {
        level = default;
        carType = default;
        if (!PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Level", out object levelObj))
        {
            Debug.LogError("Failed to retrieve level from room properties.");
            return false;
        }

        level = (LevelTypes)levelObj;

        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("CarType", out object carTypeObj))
        {
            Debug.LogError("Failed to retrieve car type from player properties.");
            return false;
        }

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
        Transform carSpawnPos = FindFreeSpawnPoint(allChildren);

        if (carSpawnPos == null)
        {
            Debug.LogError("No free spawn position found in the scene.");
            return;
        }

        Car = PhotonNetwork.Instantiate(carPrefab.name, carSpawnPos.position, carSpawnPos.rotation);
        Car.GetComponent<CarController>().Init(inputSystem);

        MarkSpawnPointAsUsed(carSpawnPos.name);

        UnityEngine.Object.Destroy(carSpawnPos.gameObject);
    }
    private void MarkSpawnPointAsUsed(string spawnPointName)
    {
        var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        string usedPoints = roomProperties.TryGetValue("UsedSpawnPoints", out object usedPointsObj)
            ? (string)usedPointsObj
            : string.Empty;

        if (!usedPoints.Contains(spawnPointName))
        {
            usedPoints = string.IsNullOrEmpty(usedPoints)
                ? spawnPointName
                : usedPoints + "," + spawnPointName;

            var newProperties = new Hashtable
        {
            { "UsedSpawnPoints", usedPoints }
        };
            PhotonNetwork.CurrentRoom.SetCustomProperties(newProperties);
        }
    }
    private bool IsSpawnPointUsed(string spawnPointName)
    {
        var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (roomProperties.TryGetValue("UsedSpawnPoints", out object usedPointsObj))
        {
            string usedPoints = (string)usedPointsObj;

            return usedPoints.Split(',').Contains(spawnPointName);
        }

        return false;
    }
    private Transform FindFreeSpawnPoint(Transform[] allChildren)
    {
        foreach (Transform child in allChildren)
        {
            if (child.CompareTag("SpawnPos") && !IsSpawnPointUsed(child.name))
            {
                return child;
            }
        }
        return null;
    }

    private void ConfigureVirtualCamera()
    {
        if (Car == null) return;
        _virtualCamera.LookAt = Car.transform;
        _virtualCamera.Follow = Car.transform;
    }
}