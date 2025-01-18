using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarDataManager _carDataManager;
    [SerializeField] private CarMovementManager _carMovementManager;
    [SerializeField] private CarEffectManager _carEffectManager;

    private CarInputSystem _carInputSystem = new CarInputSystem();

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
        _carDataManager.Init();
        _carInputSystem.Init();
        _carMovementManager.Init(_carDataManager.CarData, gameObject);

        if (GetComponent<CarVisualCustomizer>())
        {
            CarVisualCustomizer carVisualCustomizer = GetComponent<CarVisualCustomizer>();
            carVisualCustomizer.Init();
        }
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