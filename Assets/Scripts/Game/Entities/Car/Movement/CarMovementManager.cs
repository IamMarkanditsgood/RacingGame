using System;
using UnityEngine;

[Serializable]
public partial class CarMovementManager
{
    [Header("Wheels (FL,FR,RL,RR)")]
    [SerializeField] private Wheel[] _wheels;

    [SerializeField] private float _carSpeed;
    [SerializeField] private bool _isDrifting;

    private WheelFrictionData[] _wheelsFriction = new WheelFrictionData[4];
    private Rigidbody _carRigidbody;

    private bool _deceleratingCar;

    private float _localVelocityZ;
    private float _localVelocityX;

    private CarData _carData = new CarData();

    private EngineManager _engineManager = new EngineManager();
    private SteeringManager _steeringManager = new SteeringManager();
    private HandbrakeManager _handbrakeManager = new HandbrakeManager();

    public void Subscribe()
    {
        _engineManager.OnDriftCheckin += HandleDriftEffect;
        _handbrakeManager.OnDrift += HandleDriftEffect;
    }

    public void Unsubscribe()
    {
        _engineManager.OnDriftCheckin -= HandleDriftEffect;
        _handbrakeManager.OnDrift -= HandleDriftEffect;
    }

    public void Init(CarData carData, GameObject carBody)
    {
        _carData = carData;

        _carRigidbody = carBody.GetComponent<Rigidbody>();
        _carRigidbody.centerOfMass = carData.bodyMassCenter;

        SetCarComponents();
    }

    private void SetCarComponents()
    {
        InitWheels();
        _engineManager.Init(_carData, _wheels, _carRigidbody);
        _steeringManager.Init(_carData, _wheels);
        _handbrakeManager.Init(_carData, _wheels, _wheelsFriction);
    }
  
    public void UpdateCarData(GameObject car)
    {
        _carSpeed = (2 * Mathf.PI * _wheels[0].wheelCollider.radius * _wheels[0].wheelCollider.rpm * 60) / 1000;
        GameEvents.UpdateSpeed(_carSpeed);

        _localVelocityX = car.transform.InverseTransformDirection(_carRigidbody.velocity).x;

        _localVelocityZ = car.transform.InverseTransformDirection(_carRigidbody.velocity).z;
        _engineManager.UpdateLocalVelocityZ(_localVelocityZ);
    }

    public void CheckIdleConditions(CarInputManager carInputManager)
    {

        if (!carInputManager.IsBPressed && !carInputManager.IsFPressed)
        {
            _engineManager.ThrottleChange(0);
        }

        if (!carInputManager.IsBPressed && !carInputManager.IsFPressed && !carInputManager.IsDPressed && !_deceleratingCar)
        {
            _engineManager.StartCarDevelerating();
            _deceleratingCar = true;
        }

        if (!carInputManager.IsLPressed && !carInputManager.IsRPressed)
        {
            _steeringManager.ResetSteeringAngle();
        }
    }

    public void AnimateWheelMeshes()
    {
        foreach (var wheel in _wheels)
        {
            UpdateWheelPose(wheel.wheelCollider, wheel.wheelMesh);
        }
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, GameObject wheelMesh)
    {
        wheelCollider.GetWorldPose(out Vector3 wheelPosition, out Quaternion wheelRotation);

        wheelMesh.transform.position = wheelPosition;
        wheelMesh.transform.rotation = wheelRotation;
    }

    private void InitWheels()
    {
        for (int i = 0; i < _wheelsFriction.Length; i++)
        {
            _wheelsFriction[i] = new WheelFrictionData();
        }

        for (int i = 0; i < _wheels.Length; i++)
        {
            InitWheelFriction(_wheels[i].wheelCollider, ref _wheelsFriction[i].wheelFriction, ref _wheelsFriction[i].wextremumSlip);
        }
    }

    private void InitWheelFriction(WheelCollider collider, ref WheelFrictionCurve wheelFriction, ref float wheelFrictionExtremumSlip)
    {
        wheelFriction = new WheelFrictionCurve
        {
            extremumSlip = collider.sidewaysFriction.extremumSlip,
            extremumValue = collider.sidewaysFriction.extremumValue,
            asymptoteSlip = collider.sidewaysFriction.asymptoteSlip,
            asymptoteValue = collider.sidewaysFriction.asymptoteValue,
            stiffness = collider.sidewaysFriction.stiffness
        };

        wheelFrictionExtremumSlip = collider.sidewaysFriction.extremumSlip;
    }

    public void GoForwardHandler()
    {
        _engineManager.StopCarDevelerating();
        _deceleratingCar = false;
        _engineManager.GoForward(ref _carSpeed);
    }

    public void GoReverseHandler()
    {
        _engineManager.StopCarDevelerating();
        _deceleratingCar = false;
        _engineManager.GoReverse(ref _carSpeed);
    }

    public void TurnLeft()
    {
        _steeringManager.TurnLeft();
    }
    public void TurnRight()
    {
        _steeringManager.TurnRight();
    }
    public void HandBreackAction(bool state)
    {
        if (!_carData.canHandbrake)
            return;

        if(state)
        {
            _deceleratingCar = true;
            _handbrakeManager.HandbrakeActivate();
        }
        else
        {
            _handbrakeManager.RecoverTraction();
        }
    }

    private void HandleDriftEffect()
    {
        _isDrifting = Mathf.Abs(_localVelocityX) > 2.5f;
        CarEvents.Drift(_isDrifting);
    }
}