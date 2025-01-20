using System.Collections;
using Photon.Pun;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarDataManager _carDataManager;
    [SerializeField] private CarMovementManager _carMovementManager;
    [SerializeField] private PhotonView _photonView;

    private InputSystem _inputSystem = new InputSystem();
    private CarInputManager _carInputManager = new CarInputManager();

    private bool _isMyCare;

    private void Start()
    {
        StartCoroutine(CheckOwnership());
    }

    private IEnumerator CheckOwnership()
    {
        yield return new WaitUntil(() => _photonView.IsMine || PhotonNetwork.InRoom);

        _isMyCare = _photonView.IsMine;

        if (_isMyCare)
        {
            Configure();
            Subscribe();
        }
    }

    private void Update()
    {
        if (_isMyCare)
        {
            _carMovementManager.UpdateCarData(gameObject);

            _inputSystem.UpdateInputs();
            CheckInputs();
            _carMovementManager.CheckIdleConditions(_carInputManager);

            _carMovementManager.AnimateWheelMeshes();
        }
    }

    private void OnDisable()
    {
        if (_isMyCare)
        {
            UnSubscribe();
        }
    }

    private void OnDestroy()
    {
        if (_isMyCare)
        {
            UnSubscribe();
        }
    }

    public void Init(InputSystem inputSystem)
    {
        _inputSystem = inputSystem;
    }

    private void Configure()
    {   
        _carDataManager.Init();
        _inputSystem.Init();
        _carMovementManager.Init(_carDataManager.CarData, gameObject);

        ConfigureCarEffects();
        ConfigureCarVisualCustomizer();
    }

    private void ConfigureCarEffects()
    {
        if (GetComponent<CarEffectManager>())
        {
            CarEffectManager carEffectManager = GetComponent<CarEffectManager>();
            carEffectManager.Init(_photonView);
        }
    }

    private void ConfigureCarVisualCustomizer()
    {

        if (GetComponent<CarVisualCustomizer>())
        {
            CarVisualCustomizer carVisualCustomizer = GetComponent<CarVisualCustomizer>();
            carVisualCustomizer.Init();
        }
    }

    private void Subscribe()
    {
        _carMovementManager.Subscribe();
        _carInputManager.Subscribe();
    }

    private void UnSubscribe()
    {
        _carMovementManager.Unsubscribe();
        _carInputManager.Unsubscribe();
    }

    private void CheckInputs()
    {
        if(_carInputManager.IsFPressed)
        {
            _carMovementManager.GoForwardHandler();
        }
        if (_carInputManager.IsBPressed)
        {
            _carMovementManager.GoReverseHandler();
        }
        if (_carInputManager.IsLPressed)
        {
            _carMovementManager.TurnLeft();
        }
        if (_carInputManager.IsRPressed)
        {
            _carMovementManager.TurnRight();
        }

        _carMovementManager.HandBreackAction(_carInputManager.IsDPressed);
    }
}