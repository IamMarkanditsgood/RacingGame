using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private ResourcesManager _resourcesManager;
    [SerializeField] private CarDataManager _carDataManager;

    private void Awake()
    {  
        InitScene();
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        _carDataManager.Subscribe();
    }

    private void UnSubscribe()
    {
        _carDataManager.Unsubscribe();
    }

    private void InitScene()
    {
        _resourcesManager.Init();
        _carDataManager.Init();
    }
}