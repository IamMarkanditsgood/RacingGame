using System;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

[Serializable]
public class SceneCollector
{
    [SerializeField] private Transform _sceneSpawnPos;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    public GameObject Car { get; private set; }
    public GameObject Scene { get; private set; }

    public void CollectScene(GameSceneConfig gameSceneConfig)
    {
        // Отримуємо рівень і тип машини з властивостей кімнати
        var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        if (!roomProperties.TryGetValue("Level", out object levelObj) ||
            !roomProperties.TryGetValue("CarType", out object carTypeObj))
        {
            Debug.LogError("Failed to retrieve level or car type from room properties.");
            return;
        }

        var level = (LeveTypes)levelObj;
        var carType = (CarTypes)carTypeObj;
        Debug.Log("carType " + carType);

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
        Debug.Log("carPrefab " + carPrefab);
        SpawnCar(Scene, carPrefab);

        ConfigureVirtualCamera();
    }

    private GameObject InstantiateScene(GameObject scenePrefab)
    {
        return UnityEngine.Object.Instantiate(scenePrefab, _sceneSpawnPos.position, _sceneSpawnPos.rotation);
    }

    private void SpawnCar(GameObject scene, GameObject carPrefab)
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
        Debug.Log("carPrefab " + carPrefab);
        Car = PhotonNetwork.Instantiate(carPrefab.name, carSpawnPos.position, carSpawnPos.rotation);
        UnityEngine.Object.Destroy(carSpawnPos.gameObject);
    }

    private void ConfigureVirtualCamera()
    {
        if (Car == null) return;
        _virtualCamera.LookAt = Car.transform;
        _virtualCamera.Follow = Car.transform;
    }
}
