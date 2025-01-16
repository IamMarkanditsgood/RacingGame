using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarDataInitializer _carDataInitializer;
    [SerializeField] private CarMovementManager _carMovementManager;
    [SerializeField] private CarEffectManager _carEffectManager;

    private CarInputSystem _carInputSystem = new CarInputSystem();

    private CarData _carData;

    private void Start()
    {
        Init();
        Subscribe();
    }

    private void Update()
    {      
        _carMovementManager.UpdateCarData(gameObject);

        _carInputSystem.CheckInput();

        _carMovementManager.CheckIdleConditions(_carInputSystem);
        _carMovementManager.AnimateWheelMeshes();
    }

    private void OnDisable()
    {
        UnSubscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    public void Init()
    {
        _carDataInitializer.InitData(ref _carData);
        _carInputSystem.Init();
        _carMovementManager.Init(_carData, gameObject);
    }

    private void Subscribe()
    {
        _carMovementManager.Subscribe();
        _carEffectManager.Subscribe();
    }

    private void UnSubscribe()
    {
        _carMovementManager.Unsubscribe();
        _carEffectManager.UnSubscribe();
    }
}