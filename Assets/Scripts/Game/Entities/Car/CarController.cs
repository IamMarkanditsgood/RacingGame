using System.Collections;
using Photon.Pun;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private CarDataManager _carDataManager;
    [SerializeField] private CarMovementManager _carMovementManager;
    [SerializeField] private CarEffectManager _carEffectManager;
    [SerializeField] private IsMine _isMime;
    [SerializeField] private PhotonView _photonView;

    private CarInputSystem _carInputSystem = new CarInputSystem();

    public  bool _isMyCare;

    private void Start()
    {
        StartCoroutine(CheckOwnership());
    }

    private IEnumerator CheckOwnership()
    {
        yield return new WaitUntil(() => _photonView.IsMine || PhotonNetwork.InRoom);

        _isMyCare = _isMime.IsItMe(_photonView);

        if (_isMyCare)
        {
            Init();
            Subscribe();
        }
    }

    private void Update()
    {
        if (_isMyCare)
        {
            _carMovementManager.UpdateCarData(gameObject);

            _carInputSystem.CheckInput();

            _carMovementManager.CheckIdleConditions(_carInputSystem);
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